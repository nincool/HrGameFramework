using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using Hr.CommonUtility;
using System.Text.RegularExpressions;

namespace Hr.Resource
{
    public enum EnumHrAssetLoadMode
    {
        STREAMING,
        PERSISTENT,
    }

    public class HrResourceManager : HrUnitySingleton<HrResourceManager>
    {
        /// <summary>
        /// 资源加载模式
        /// </summary>
        public const EnumHrAssetLoadMode m_assetLoadMode = EnumHrAssetLoadMode.PERSISTENT;

        private string m_strVariant = "";

        private List<string> m_lisAssetBundleName = new List<string>();
        private List<string> m_lisAssetBundleWithVariantName = new List<string>();
        private Dictionary<string, List<string>> m_dicAssetDependicesInfo = new Dictionary<string, List<string>>();

        private Dictionary<string, HrAssetBundle> m_dicAssetBundleInfo = new Dictionary<string, HrAssetBundle>();
        private Dictionary<string, HrAssetBundle> m_dicAssetInAssetBundleInfo = new Dictionary<string, HrAssetBundle>();

        public Dictionary<string, HrAssetBundle> AssetBundlePool
        {
            get { return m_dicAssetBundleInfo; }
        }

        public string Variant
        {
            set { m_strVariant = value; }
            get { return m_strVariant; }
        }

        public void Init()
        {

        }

        public List<string> GetAssetBundleDependices(string strAssetBundleName)
        {
            List<string> lisRt = null;
            m_dicAssetDependicesInfo.TryGetValue(strAssetBundleName, out lisRt);

            return lisRt;
        }

        public bool LoadAssetBundleManifest()
        {
            string strAssetBundleMenifestPath = HrAssetBundleUtility.GetAssetBundleManifestPath();
            if (!File.Exists(strAssetBundleMenifestPath))
            {
                Debug.LogError("LoadAssetBundleManifest Error! Is not existed! ManifestPath:" + strAssetBundleMenifestPath);
                return false;
            }
            AssetBundle assetBundle = AssetBundle.LoadFromFile(strAssetBundleMenifestPath);
            AssetBundleManifest manifest = assetBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
            assetBundle.Unload(false);

            var strAllAssetBundles = manifest.GetAllAssetBundles();
            if (!string.IsNullOrEmpty(m_strVariant))
            {
                strAllAssetBundles = strAllAssetBundles.Where(o => HrFileUtil.GetFileSuffix(o) == m_strVariant).ToArray<string>();
            }
            foreach (var strAssetName in strAllAssetBundles)
            {
                var lisDependicesArr = manifest.GetAllDependencies(strAssetName)
                    .Select(o =>  o.ToLower() ).ToList<string>();
                m_dicAssetDependicesInfo.Add(strAssetName, lisDependicesArr);
            }
            m_lisAssetBundleName.AddRange(strAllAssetBundles.ToList<string>());

            var strAllAssetWithVariant = manifest.GetAllAssetBundlesWithVariant();
            if (!string.IsNullOrEmpty(m_strVariant))
            {
                strAllAssetWithVariant = strAllAssetWithVariant.Where(o => HrFileUtil.GetFileSuffix(o) == m_strVariant).ToArray<string>();
            }
            m_lisAssetBundleWithVariantName.AddRange(strAllAssetWithVariant);

            return true;
        }

        public void LoadAssetBundleSync(string strAssetBundleName)
        {
#if UNITY_EDITOR
            if (Regex.IsMatch(strAssetBundleName, "[A-Z]"))
            {
                HrLoger.LogError("LoadAssetBundleSync Error! AssetBundleName has upper case!");
            }
#endif
            HrAssetBundle loadedAssetBundle = null;
            m_dicAssetBundleInfo.TryGetValue(strAssetBundleName, out loadedAssetBundle);
            if (loadedAssetBundle != null)
            {
                while (loadedAssetBundle.IsLoading() && !loadedAssetBundle.IsError()) { }
                
                Debug.LogError("HrResourceManager:LoadAssetBundleSync Error! AssetBundleName:" + strAssetBundleName);
                return;

            }

            string strAssetBundleFullPath = HrResourcePath.CombineAssetBundlePath(strAssetBundleName);

            if (!File.Exists(strAssetBundleFullPath))
            {
                Debug.LogError("HrResourceManager:LoadAssetBundleSync Error! AssetBunde is not exist!:" + strAssetBundleFullPath);
                return;
            }

            HrAssetBundle assetBundle = new HrAssetBundle(strAssetBundleName, strAssetBundleFullPath, new Action<HrAssetBundle>(ActionAssetBundleLoadFinished));
            assetBundle.LoadSync();
        }

        public void ActionAssetBundleLoadFinished(HrAssetBundle assetBundle)
        {
            Debug.Log("ActionAssetBundleLoadFinished AssetBundle:" + assetBundle.Name);

            m_dicAssetBundleInfo.Add(assetBundle.Name, assetBundle);

            var strAssetNameArr = assetBundle.MonoAssetBundle.GetAllAssetNames();
            foreach (var strAsset in strAssetNameArr)
            {
                m_dicAssetInAssetBundleInfo.Add(strAsset, assetBundle);
            }
        }

        public T GetAsset<T>(string strAssetName) where T : UnityEngine.Object
        {
            HrAssetBundle assetBundleInfo = null;
            m_dicAssetInAssetBundleInfo.TryGetValue(strAssetName, out assetBundleInfo);
            if (assetBundleInfo != null)
            {
                return assetBundleInfo.MonoAssetBundle.LoadAsset<T>(strAssetName);
            }
            else
            {
                HrLoger.LogError("HrResourceManager:: GetAsset Error! AssetName:" + strAssetName);
            }

            return null;
        }


    }
}

