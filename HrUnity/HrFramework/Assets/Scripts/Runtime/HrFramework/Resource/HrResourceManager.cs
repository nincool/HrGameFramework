using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using Hr;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

namespace Hr.Resource
{
    public enum EnumHrAssetLoadMode
    {
        STREAMING,
        PERSISTENT,
    }

    public class HrResourceManager : HrModule, IResourceManager
    {
        /// <summary>
        /// 资源加载模式
        /// </summary>
        public const EnumHrAssetLoadMode m_assetLoadMode = EnumHrAssetLoadMode.PERSISTENT;

        private string m_strVariant = "";

        /// <summary>
        /// 所有AssetBundle资源的名称
        /// </summary>
        private List<string> m_lisAssetBundleName = new List<string>();
        
        /// <summary>
        /// 资源的依赖资源
        /// </summary>
        private Dictionary<string, List<string>> m_dicAssetDependicesInfo = new Dictionary<string, List<string>>();

        /// <summary>
        /// 单个资源所在的AssetBundle名称
        /// </summary>
        private Dictionary<string, string> m_dicAssetInAssetBundleName = new Dictionary<string, string>();
        
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

        /// <summary>
        /// 暂时不用
        /// </summary>
        public string Variant
        {
            set { m_strVariant = value; }
            get { return m_strVariant; }
        }

        protected HrLoadAssetEvent m_loadAssetEvent = new HrLoadAssetEvent();

        public override void Init()
        {
            HrResourcePrefab.RegisterType();
        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {

        }

        public static void AddResourceType(System.Object unityType, System.Object assetType)
        {
            ms_dicUnityType2AssetType.Add(unityType, assetType);
        }

        /// <summary>
        /// 获取AssetBundle的依赖
        /// </summary>
        /// <param name="strAssetBundleName"></param>
        /// <returns>AssetBundle依赖的集合</returns>
        public List<string> GetAssetBundleDependices(string strAssetBundleName)
        {
            List<string> lisRt = null;
            m_dicAssetDependicesInfo.TryGetValue(strAssetBundleName, out lisRt);

            return lisRt;
        }

        private bool LoadAssetBundleManifest()
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

            //获取所有的AssetBundle
            var strAllAssetBundles = manifest.GetAllAssetBundles();
            if (!string.IsNullOrEmpty(m_strVariant))
            {
                strAllAssetBundles = strAllAssetBundles.Where(o => (HrFileUtil.GetFileSuffix(o) == "" || HrFileUtil.GetFileSuffix(o) == m_strVariant)).ToArray<string>();
            }
            //查找每个AssetBundle的依赖项
            foreach (var strAssetName in strAllAssetBundles)
            {
                var lisDependicesArr = manifest.GetAllDependencies(strAssetName)
                    .Select(o =>  o.ToLower() ).ToList<string>();
                if (lisDependicesArr.Count > 0)
                    m_dicAssetDependicesInfo.Add(strAssetName, lisDependicesArr);
            }
            m_lisAssetBundleName.AddRange(strAllAssetBundles.ToList<string>());

            ///todo 解析后写入配置文件
            //解析每个AssetBundle的manifest文件，收集里面打入的资源名称 转换为小写
            foreach (var strAssetBundleName in m_lisAssetBundleName)
            {
                string strAssetBundleFullPath = HrResourcePath.CombineAssetBundlePath(strAssetBundleName);
                string strAssetBundleManifest = strAssetBundleFullPath + ".manifest";
                if (!File.Exists(strAssetBundleManifest))
                {
                    Debug.LogError("HrResourceManager:LoadAssetBundleManifest error! AssetBundleManifest:" + strAssetBundleManifest + " is not existed!");
                    continue;
                }
                StreamReader fs = new StreamReader(strAssetBundleManifest);
                string strLine;
                bool bStartAssets = false;
                while ((strLine = fs.ReadLine()) != null)
                {
                    strLine = strLine.Replace("\n", "");
                    if (!bStartAssets && strLine.IndexOf("Assets:") != -1)
                    {
                        bStartAssets = true;
                        continue;
                    }
                    if (bStartAssets)
                    {
                        strLine = strLine.Replace("- ", "").ToLower();
                        m_dicAssetInAssetBundleName.Add(strLine, strAssetBundleName);
                    }
                    if (strLine.IndexOf("Dependencies:") != -1)
                    {
                        bStartAssets = false;
                        break;
                    }
                }
            }

            return true;
        }

        public void LoadSceneSync(string strSceneName, string strAssetBundleName)
        {
            HrAssetBundle assetBundle = LoadAssetBundleSync(strAssetBundleName);
            SceneManager.LoadScene(strSceneName, LoadSceneMode.Additive);
        }

        /// <summary>
        /// 手动加载AssetBundle
        /// </summary>
        /// <param name="strAssetBundle"></param>
        public HrAssetBundle LoadAssetBundleSync(string strAssetBundleName)
        {
            HrAssetBundle loadedAssetBundle = m_dicAssetBundleInfo.HrTryGet(strAssetBundleName);
            if (loadedAssetBundle != null)
            {
                if (loadedAssetBundle.IsLoaded())
                {
                    return loadedAssetBundle;
                }

                HrLogger.LogWaring(string.Format("LoadAssetBunldeSyn AssetBundle is no null! AssetBundle[{0}]", strAssetBundleName));
                ///todo 1.判断是否正在加载 2.加载 3.等待加载
                while (loadedAssetBundle.IsLoading() && !loadedAssetBundle.IsError()) { }

                return loadedAssetBundle;
            }

            string strAssetBundleFullPath = HrResourcePath.CombineAssetBundlePath(strAssetBundleName);
            if (!File.Exists(strAssetBundleFullPath))
            {
                HrLogger.LogError(string.Format("assetbundle is not existed! [{0}]", strAssetBundleFullPath));
                return null;
            }

            HrAssetBundle assetBundle = new HrAssetBundle(strAssetBundleName, strAssetBundleFullPath);
            assetBundle.LoadAssetBundleEvent.LoadAssetSuccessHandler += LoadAssetBundleSuccessHandler;
            assetBundle.LoadAssetBundleEvent.LoadAssetFailedHandler += LoadAssetBundleFailedHandler;
            assetBundle.LoadAssetBundleEvent.LoadAssetProgressHandler += LoadAssetBundleProgressHandler;
            assetBundle.LoadSync();

            return assetBundle;
        }

        private void LoadAssetBundleSuccessHandler(object sender, EventArgs args)
        {
            HrLoadAssetSuccessEventArgs eventArgs = args as HrLoadAssetSuccessEventArgs;
            HrLogger.Log(string.Format("LoadAssetBundleSuccess! assetBundle:{0} duration:{1}", eventArgs.AssetName,  eventArgs.Duration));
            HrAssetBundle assetBundle = eventArgs.UserData as HrAssetBundle;

            ParseAssetBundle(assetBundle);
        }

        public void LoadAssetBundleFailedHandler(object sender, EventArgs args)
        {

        }

        public void LoadAssetBundleProgressHandler(object sender, EventArgs args)
        {

        }

        private void ParseAssetBundle(HrAssetBundle assetBundle)
        {
            m_dicAssetBundleInfo.Add(assetBundle.Name, assetBundle);

            var strAllAssetsNameArr = assetBundle.MonoAssetBundle.GetAllAssetNames();
            foreach (var strAssetName in strAllAssetsNameArr)
            {
                var o = assetBundle.MonoAssetBundle.LoadAsset(strAssetName);
                //判断类型
                System.Type type = ms_dicUnityType2AssetType.HrTryGet(o.GetType()) as System.Type;
                if (type != null)
                {
                    HrResource res = Activator.CreateInstance(type, new object[] { strAssetName, o, assetBundle }) as HrResource;
                    if (res == null)
                    {
                        HrLogger.LogError("ActionAssetBundleLoadFinished Error! assetName:" + o.name + " AssetBundle:" + assetBundle.Name);
                    }
                    else
                    {
                        m_dicItemResourceInfo.Add(strAssetName, res);
                    }
                }
                else
                {
                    HrLogger.LogError("ActionAssetBundleLoadFinished Error! Can not find the asset type:" + o.name);
                }
            }
        }

        /// <summary>
        /// 同步加载资源
        /// </summary>
        /// <param name="strAssetBundleName">加载的AssetBundle名称</param>
        /// <returns>返回加载的HrAssetBundle对象</returns>
//        public HrAssetBundle LoadAssetBundleSync(string strAssetBundleName)
//        {
//#if UNITY_EDITOR
//            if (Regex.IsMatch(strAssetBundleName, "[A-Z]"))
//            {
//                HrLogger.LogError("LoadAssetBundleSync Error! AssetBundleName has upper case!");
//            }
//#endif
//            HrAssetBundle loadedAssetBundle = null;
//            m_dicAssetBundleInfo.TryGetValue(strAssetBundleName, out loadedAssetBundle);
//            if (loadedAssetBundle != null)
//            {
//                while (loadedAssetBundle.IsLoading() && !loadedAssetBundle.IsError()) { }
                
//                Debug.LogError("HrResourceManager:LoadAssetBundleSync Error! AssetBundleName:" + strAssetBundleName);

//                return null;

//            }

//            string strAssetBundleFullPath = HrResourcePath.CombineAssetBundlePath(strAssetBundleName);

//            if (!File.Exists(strAssetBundleFullPath))
//            {
//                Debug.LogError("HrResourceManager:LoadAssetBundleSync Error! AssetBunde is not exist!:" + strAssetBundleFullPath);

//                return null;
//            }

//            HrAssetBundle assetBundle = new HrAssetBundle(strAssetBundleName, strAssetBundleFullPath, m_loadAssetEvent);
//            assetBundle.LoadSync();

//            return assetBundle;
//        }



        /// <summary>
        /// 获取单个资源，如果对应的AssetBundle没有加载，那么同步加载AssetBundle
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="strAssetPath">资源的路径</param>
        /// <returns>资源对象</returns>
        public T LoadAsset<T>(string strAssetPath)
        {
            string strAssetBundleName = m_dicAssetInAssetBundleName.HrTryGet(strAssetPath);
            if (string.IsNullOrEmpty(strAssetBundleName))
            {
                HrLogger.LogError("LoadAsset Error! Do not find the assetBundleName Asset:" + strAssetPath);
                return default(T);
            }
            HrAssetBundle assetBundle = LoadAssetBundleSync(strAssetBundleName);
            if (assetBundle == null)
            {
                HrLogger.LogError("LoadAsset Error! AssetBundle is null Asset:" + strAssetPath);
                return default(T);
            }
            //同步加载肯定保证已经是加载状态
            HrResource res = m_dicItemResourceInfo.HrTryGet(strAssetPath);
            if (res == null)
            {
                HrLogger.LogError("LoadAsset Error! Resource is null Asset:" + strAssetPath);
                return default(T);
            }
            System.Type type = ms_dicUnityType2AssetType.HrTryGet(typeof(T)) as System.Type;

            return ((T)(res.m_unityAsset as object));
        }
    }
}

