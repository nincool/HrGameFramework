using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using Hr;
using System.Text.RegularExpressions;

namespace Hr
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

        /// <summary>
        /// 所有资源的名称
        /// </summary>
        private List<string> m_lisAssetBundleName = new List<string>();
        
        /// <summary>
        /// 所有带有Variant资源的名称
        /// </summary>
        private List<string> m_lisAssetBundleWithVariantName = new List<string>();

        /// <summary>
        /// 资源的依赖资源
        /// </summary>
        private Dictionary<string, List<string>> m_dicAssetDependicesInfo = new Dictionary<string, List<string>>();

        /// <summary>
        /// 加载的AssetBundle
        /// </summary>
        private Dictionary<string, HrAssetBundle> m_dicAssetBundleInfo = new Dictionary<string, HrAssetBundle>();

        /// <summary>
        /// 具体的资源信息
        /// </summary>
        private Dictionary<string, HrResource> m_dicItemResourceInfo = new Dictionary<string, HrResource>();

        /// <summary>
        /// 资源对应的Resource类型
        /// </summary>
        private static Dictionary<System.Object, System.Object> ms_dicUnityType2AssetType = new Dictionary<object, object>();

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

        public static void AddResourceType(System.Object unityType, System.Object assetType)
        {
            ms_dicUnityType2AssetType.Add(unityType, assetType);
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
                HrLogger.LogError("LoadAssetBundleSync Error! AssetBundleName has upper case!");
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

            var assetObjectArr = assetBundle.MonoAssetBundle.LoadAllAssets();
            foreach (var o in assetObjectArr)
            {
                //判断类型
                System.Type type = ms_dicUnityType2AssetType.HrTryGet(o.GetType()) as System.Type;
                if (type != null)
                {
                    HrResource res = Activator.CreateInstance(type, new object[] { o.name, assetBundle}) as HrResource;
                    if (res == null)
                    {
                        HrLogger.LogError("ActionAssetBundleLoadFinished Error! assetName:" + o.name + " AssetBundle:" + assetBundle.Name);
                    }
                    else
                    {
                        m_dicItemResourceInfo.Add(o.name, res);
                    }
                }
            }
        }

        /// <summary>
        /// 加载AssetBundle中的资源 如果AssetBundle都没有加载那么同步加载AssetBundle
        /// </summary>
        public void LoadAsset(string strAssetPath)
        {

        }
    }
}

