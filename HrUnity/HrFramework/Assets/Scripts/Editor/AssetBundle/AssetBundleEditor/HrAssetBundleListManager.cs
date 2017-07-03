using Hr.Editor.Hierarchy;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;
using LitJson;
using System.IO;
using System.Text;
using System;
using Hr.Utility;


namespace Hr.Editor
{

    public class HrAssetBundleListManager
    {
        private const string m_c_strConfigurationName = "HrFramework/Configs/AssetBundleCollection.json";

        private const string m_c_strAssetBundleNamePattern = @"^([A-Za-z0-9\._-]+/)*[A-Za-z0-9\._-]+$";
        private const string m_c_strAssetBundleVariantPattern = @"^[a-z0-9_-]+$";

        private HrAssetBundleHierarchy m_fileHierarchy;

        public HrAssetBundleAssignManager EditorManager
        {
            get;
            private set;
        }

        public HrAssetBundleHierarchy FileHierarchy
        {
            get
            {
                return m_fileHierarchy;
            }
        }

        public HrAssetBundleListManager(HrAssetBundleAssignManager editorManager)
        {
            EditorManager = editorManager;
        }

        public bool Init()
        {

            return true;
        }

        public bool Load()
        {
            string strConfigurationName = HrFileUtil.GetCombinePath(Application.dataPath, m_c_strConfigurationName);
            if (!File.Exists(strConfigurationName))
            {
                return false;
            }
            m_fileHierarchy = new HrAssetBundleHierarchy("AssetBundles");

            string strData = File.ReadAllText(strConfigurationName);
            JsonData jsonData = JsonMapper.ToObject(strData);
            IDictionary dicJsonData = jsonData as IDictionary;
            if (dicJsonData == null)
            {
                return false;
            }

            int nAssetBundleIndex = 0;
            while (true)
            {
                string strAssetBundle = string.Format("AssetBundle_{0}", nAssetBundleIndex);
                if (dicJsonData.Contains(strAssetBundle))
                {
                    string strName = jsonData[strAssetBundle]["Name"].ToString();
                    string strVariant = jsonData[strAssetBundle]["Variant"].ToString();
                    if (!AddAssetBundle(strName, strVariant))
                    {
                        string strAssetBundleFullName = !string.IsNullOrEmpty(strVariant) ? string.Format("{0}.{1}", strName, strVariant) : strName;
                        Debug.LogError(string.Format("Can not add AssetBundle '{0}'.", strAssetBundleFullName));
                    }
                }
                else
                {
                    break;
                }
                ++nAssetBundleIndex;
            }

            int nAssetIndex = 0;
            while (true)
            {
                string strAsset = string.Format("Asset_{0}", nAssetIndex);
                if (dicJsonData.Contains(strAsset))
                {
                    string strGUID = jsonData[strAsset]["GUID"].ToString();
                    string strAssetBundleName = jsonData[strAsset]["AssetBundleName"].ToString();
                    string strAssetBundleVariant = jsonData[strAsset]["AssetBundleVariant"].ToString();
                    if (!AssignFile(strAssetBundleName, strAssetBundleVariant, strGUID))
                    {
                        string strAssetBundleFullName = !string.IsNullOrEmpty(strAssetBundleVariant) ? string.Format("{0}.{1}", strAssetBundleName, strAssetBundleVariant) : strAssetBundleName;
                        Debug.LogError(string.Format("Can not assign asset '{0}' to AssetBundle '{1}'", strGUID, strAssetBundleFullName));
                    }
                }
                else
                {
                    break;
                }
                ++nAssetIndex;
            }

            EditorManager.SourceListManager.FileHierarchy.RefreshHierarchy();

            return true;

        }

        public bool Save()
        {
            string strConfigurationName = HrFileUtil.GetCombinePath(Application.dataPath, m_c_strConfigurationName);

            if (File.Exists(strConfigurationName))
            {

                string strOldConfigurationFileName = HrFileUtil.GetFileNameWithoutSuffix(m_c_strConfigurationName) + HrTimeUtils.DateTime2DateTimeStr(DateTime.Now).Replace(':', '-') + ".json";
                string strDirectoryPath = Path.GetDirectoryName(strConfigurationName) + "/Backup";
                string strOldConfigurationName = HrFileUtil.GetCombinePath(strDirectoryPath, strOldConfigurationFileName);

                File.Move(strConfigurationName, strOldConfigurationName);
            }

            JsonWriter writer = new JsonWriter();

            writer.WriteObjectStart();

            var allAssetBundles = m_fileHierarchy.AssetBundleFolder.GetAssetBundles();
            var allFiles = m_fileHierarchy.AssetBundleFolder.GetAssets();
            int nAssetBundleIndex = 0;
            foreach (HrAssetBundle assetBundle in allAssetBundles)
            {
                writer.WritePropertyName(string.Format("AssetBundle_{0}", nAssetBundleIndex));
                writer.WriteObjectStart();

                writer.WritePropertyName("Name");
                writer.Write(assetBundle.Name);
                writer.WritePropertyName("Variant");
                writer.Write(assetBundle.Variant);


                writer.WriteObjectEnd();

                ++nAssetBundleIndex;
            }

            int nAssetIndex = 0;
            foreach (HrFileItem asset in allFiles)
            {
                writer.WritePropertyName(string.Format("Asset_{0}", nAssetIndex));
                writer.WriteObjectStart();

                writer.WritePropertyName("GUID");
                writer.Write(asset.Guid);
                writer.WritePropertyName("AssetBundleName");
                writer.Write(asset.AssetBundle.Name);
                writer.WritePropertyName("AssetBundleVariant");
                writer.Write(asset.AssetBundle.Variant);

                writer.WriteObjectEnd();

                ++nAssetIndex;
            }

            writer.WriteObjectEnd();

            File.WriteAllText(strConfigurationName, writer.ToString(), Encoding.UTF8);

            return true;

        }

        public bool AddAssetBundle(string strAssetBundleName, string strAssetBundleVariant)
        {
            if (!IsValidAssetBundleName(strAssetBundleName, strAssetBundleVariant))
            {
                return false;
            }

            if (IsAvailableBundleName(strAssetBundleName, strAssetBundleVariant, null))
            {
                return false;
            }

            HrAssetBundle assetBundle = HrAssetBundle.Create(strAssetBundleName, strAssetBundleVariant);
            m_fileHierarchy.AssetBundleFolder.AssetBundles.Add(assetBundle.FullName, assetBundle);

            return true;
        }

        /// <summary>
        /// AssetBundle的完整名称，和Unity打出来的AssetBundle一致 name.variant
        /// </summary>
        /// <param name="strAssetBundleName"></param>
        /// <param name="strAssetBundleVariant"></param>
        /// <returns></returns>
        private string GetAssetBundleFullName(string strAssetBundleName, string strAssetBundleVariant)
        {
            return !string.IsNullOrEmpty(strAssetBundleVariant) ? string.Format("{0}.{1}", strAssetBundleName, strAssetBundleVariant) : strAssetBundleName;
        }

        /// <summary>
        /// AssetBundle的名字是否合法
        /// </summary>
        /// <param name="strAssetBundleName"></param>
        /// <param name="strAssetBundleVariant"></param>
        /// <returns></returns>
        private bool IsValidAssetBundleName(string strAssetBundleName, string strAssetBundleVariant)
        {
            if (string.IsNullOrEmpty(strAssetBundleName))
            {
                return false;
            }

            if (!Regex.IsMatch(strAssetBundleName, m_c_strAssetBundleNamePattern))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(strAssetBundleVariant) && !Regex.IsMatch(strAssetBundleVariant, m_c_strAssetBundleVariantPattern))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 查看名字是否存在
        /// </summary>
        /// <param name="strAssetBundleName"></param>
        /// <param name="strAssetBundleVariant"></param>
        /// <param name="selfAssetBundle">需要校验的AssetBundle</param>
        /// <returns></returns>
        private bool IsAvailableBundleName(string strAssetBundleName, string strAssetBundleVariant, HrAssetBundle selfAssetBundle)
        {
            HrAssetBundle findAssetBundle = GetAssetBundle(strAssetBundleName, strAssetBundleVariant);
            if (findAssetBundle != null)
            {
                return findAssetBundle == selfAssetBundle;
            }

            foreach (HrAssetBundle assetBundle in m_fileHierarchy.AssetBundleFolder.AssetBundles.Values)
            {
                if (selfAssetBundle != null && assetBundle == selfAssetBundle)
                {
                    continue;
                }

                if (assetBundle.Name == strAssetBundleName)
                {
                    if (assetBundle.Variant != null && strAssetBundleVariant == assetBundle.Variant)
                    {
                        return true;
                    }

                    if (assetBundle.Variant == null && strAssetBundleVariant == null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public HrAssetBundle GetAssetBundle(string strAssetBundleName, string strAssetBundleVariant)
        {
            if (!IsValidAssetBundleName(strAssetBundleName, strAssetBundleVariant))
            {
                return null;
            }

            HrAssetBundle assetBundle = m_fileHierarchy.AssetBundleFolder.AssetBundles.HrTryGet(GetAssetBundleFullName(strAssetBundleName, strAssetBundleVariant));

            return assetBundle;
        }

        public bool AssignFile(string strAssetBundleName, string strAssetBundleVariant, HrFileItem fileItem)
        {
            var assetBundle = GetAssetBundle(strAssetBundleName, strAssetBundleVariant);
            if (assetBundle != null)
            {
                assetBundle.AssignAsset(fileItem, false);

                return true;
            }
            return false;
        }

        public bool AssignFile(string strAssetBundleName, string strAssetBundleVariant, string strGUID)
        {
            var assetBundle = GetAssetBundle(strAssetBundleName, strAssetBundleVariant);
            if (assetBundle != null)
            {
                var fileItem = EditorManager.SourceListManager.GetFileItem(strGUID);

                if (fileItem != null)
                {
                    assetBundle.AssignAsset(fileItem, false);
                }
                else
                {
                    Debug.LogError(string.Format("AssignFile Error! fileItem '{0} can not find from the assets's folder", strGUID));
                    return false;
                }
                return true;
            }
            else
            {
                Debug.LogError(string.Format("AssignFile Error! can not fine the assetbundle '{0}.{1}'", strAssetBundleName, strAssetBundleVariant));
            }
            return false;
        }

        public bool UnassignFile(string strAssetBundleName, string strAssetBundleVariant, HrFileItem fileItem)
        {
            var assetBundle = GetAssetBundle(strAssetBundleName, strAssetBundleVariant);
            if (assetBundle != null)
            {
                assetBundle.Unassign(fileItem);
            }
            else
            {
                Debug.LogError(string.Format("UnassignFile Error! can not find assetbundle '{0}.{1}'", strAssetBundleName, strAssetBundleVariant));

                return false;
            }

            return true;
        }

        public bool RemoveAssetBundle(string strAssetBundleName, string strAssetBundleVariant)
        {
            if (!IsValidAssetBundleName(strAssetBundleName, strAssetBundleVariant))
            {
                return false;
            }

            string strAssetBundleFullName = GetAssetBundleFullName(strAssetBundleName, strAssetBundleVariant);
            return FileHierarchy.RemoveAssetBundle(strAssetBundleFullName);
        }

        private void OnRemoveAssetBundle(HrAssetBundle assetBundle)
        {
            
        }
    }
}
