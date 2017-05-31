using Hr.Editor.Hierarchy;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Hr.Editor
{
    public class HrAssetBundleContainer
    {
        private const string m_c_strConfigurationName = "HrFramework/Configs/AssetBundleCollection.json";

        private Dictionary<string, HrAssetBundle> m_dicAssetBundles = new Dictionary<string, HrAssetBundle>();

        public HrAssetBundleContainer()
        {

        }


        public bool Load()
        {
            string strConfigurationName = HrFileUtil.GetCombinePath(Application.dataPath, m_c_strConfigurationName);
            if (!File.Exists(strConfigurationName))
            {
                return false;
            }

            m_dicAssetBundles.Clear();

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
                        string strAssetBundleFullName = strVariant != null ? string.Format("{0}.{1}", strName, strVariant) : strName;
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
                        string strAssetBundleFullName = strAssetBundleVariant != null ? string.Format("{0}.{1}", strAssetBundleName, strAssetBundleVariant) : strAssetBundleName;
                        Debug.LogError(string.Format("Can not assign asset '{0}' to AssetBundle '{1}'", strGUID, strAssetBundleFullName));
                    }
                }
                else
                {
                    break;
                }
                ++nAssetIndex;
            }

            return true;
        }

        private bool AddAssetBundle(string strAssetBundleName, string strVariant)
        {
            HrAssetBundle assetBundle = HrAssetBundle.Create(strAssetBundleName, strVariant);
            m_dicAssetBundles.Add(assetBundle.FullName, assetBundle);

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
            return strAssetBundleVariant != null ? string.Format("{0}.{1}", strAssetBundleName, strAssetBundleVariant) : strAssetBundleName;
        }

        public HrAssetBundle GetAssetBundle(string strAssetBundleName, string strAssetBundleVariant)
        {
            return m_dicAssetBundles.HrTryGet(GetAssetBundleFullName(strAssetBundleName, strAssetBundleVariant));
        }

        public HrAssetBundle[] GetAssetBundles()
        {
            return m_dicAssetBundles.Values.ToArray();
        }

        public bool AssignFile(string strAssetBundleName, string strAssetBundleVariant, string strGUID)
        {
            var assetBundle = GetAssetBundle(strAssetBundleName, strAssetBundleVariant);
            if (assetBundle != null)
            {
                HrFileItem fileItem = new HrFileItem(strGUID, null);

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
    }

}
