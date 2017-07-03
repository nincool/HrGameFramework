using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using Hr;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using LitJson;
using Hr.Utility;

namespace Hr.Resource
{
    public class HrResourceManager : HrModule, IResourceManager
    {
        /// <summary>
        /// DataTable 对应在哪个Binary文件里
        /// </summary>
        private readonly Dictionary<string, string> m_dicDataTableInBinaryInfo = new Dictionary<string, string>();

        /// <summary>
        /// 单个资源 ID 对应的路径和 AssetBundle 名称
        /// </summary>
        private readonly Dictionary<int, KeyValuePair<string, string>> m_dicResourceID2PathAndAssetBundle = new Dictionary<int, KeyValuePair<string, string>>();

        /// <summary>
        /// 单个资源路径对应ID
        /// </summary>
        private readonly Dictionary<string, int> m_dicResourcePath2IDInfo = new Dictionary<string, int>();
        
        /// <summary>
        /// 解析的AssetFile
        /// </summary>
        private Dictionary<string, HrAssetFile> m_dicAssetFileInfo = new Dictionary<string, HrAssetFile>();

        /// <summary>
        /// 具体的资源信息
        /// </summary>
        private Dictionary<string, HrResource> m_dicItemResourceInfo = new Dictionary<string, HrResource>();

        /// <summary>
        /// 资源对应的Resource类型
        /// </summary>
        private static Dictionary<System.Object, System.Object> ms_dicUnityType2AssetType = new Dictionary<object, object>();

        public override void Init()
        {
            HrResourcePrefab.RegisterType();
        }

        public void LoadAssetsConfig()
        {
            //加载DataTable 配置
            LoadDataTableConfig();

            //加载资源信息 配置
            LoadAssetsListConfig();
        }

        public override void Shutdown()
        {
        }

 
        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {

        }

        public static void AddResourceType(System.Object unityType, System.Object assetType)
        {
            ms_dicUnityType2AssetType.Add(unityType, assetType);
        }

        public void LoadDataTable(string strDataTableName, HrLoadResourceCallBack loadResourceCallBack)
        {
            //先尝试获取
            HrResource res = m_dicItemResourceInfo.HrTryGet(strDataTableName);
            if (res != null)
            {
                if (loadResourceCallBack != null && loadResourceCallBack.LoadResourceSuccess != null)
                {
                    loadResourceCallBack.LoadResourceSuccess(res);
                }
                return;
            }

            //加载AssetBinary文件
            var strBinaryFileName = m_dicDataTableInBinaryInfo.HrTryGet(strDataTableName);
            if (strBinaryFileName == null)
            {
                string strErrorMsg = string.Format("can not find the datatable info! [{0}]", strDataTableName);
                HrLogger.LogError(strErrorMsg);
                if (loadResourceCallBack != null && loadResourceCallBack.LoadResourceFailed != null)
                {
                    loadResourceCallBack.LoadResourceFailed(strDataTableName, strErrorMsg);
                }
                return;
            }
            var assetFile = m_dicAssetFileInfo.HrTryGet(strBinaryFileName);
            if (assetFile == null)
            {
                string strErrorMsg = string.Format("can not find the asset file! [{0}]", strBinaryFileName);
                HrLogger.LogError(strErrorMsg);
                if (loadResourceCallBack != null && loadResourceCallBack.LoadResourceFailed != null)
                {
                    loadResourceCallBack.LoadResourceFailed(strDataTableName, strErrorMsg);
                }
                return;
            }
            HrAssetBinary assetBinaryFile = assetFile as HrAssetBinary;
            if (assetBinaryFile.IsLoaded())
            {
                HrLogger.LogWarning(string.Format("asset is loaded! filename [{0}]", assetBinaryFile.Name));
            }
            else
            {
                assetBinaryFile.LoadSync();
            }

            //重新尝试获取
            if (assetBinaryFile.IsLoaded())
            {
                res = m_dicItemResourceInfo.HrTryGet(strDataTableName);
                if (res == null)
                {
                    string strErrorMsg = string.Format("error! invalid res!!! [{0}]", strDataTableName);
                    HrLogger.LogError(strErrorMsg);
                    if (loadResourceCallBack != null && loadResourceCallBack.LoadResourceFailed != null)
                    {
                        loadResourceCallBack.LoadResourceFailed(strDataTableName, strErrorMsg);
                    }
                    return;
                }
                if (loadResourceCallBack != null && loadResourceCallBack.LoadResourceSuccess != null)
                {
                    loadResourceCallBack.LoadResourceSuccess(res);
                }
                return;
            }
            else
            {
                string strErrorMsg = string.Format("load binary file error! [{0}]", assetBinaryFile.Name);
                HrLogger.LogError(strErrorMsg);
                if (loadResourceCallBack != null && loadResourceCallBack.LoadResourceFailed != null)
                {
                    loadResourceCallBack.LoadResourceFailed(strDataTableName, strErrorMsg);
                }
            }

            return;
        }

        public void LoadResourceSync(int nID, HrLoadResourceCallBack loadResourceCallBack)
        {
            var resourceInfo = m_dicResourceID2PathAndAssetBundle.HrTryGet(nID);
            string strAssetPath = resourceInfo.Key;
            string strAssetBundleName = resourceInfo.Value;

            var res = m_dicItemResourceInfo.HrTryGet(strAssetPath);
            if (res == null)
            {
                LoadAssetBundleSync(strAssetBundleName, null);
                HrAssetBundle assetFile = m_dicAssetFileInfo.HrTryGet(strAssetBundleName) as HrAssetBundle;
                if (assetFile != null && assetFile.IsLoaded() && !assetFile.IsError())
                {
                    var o = assetFile.MonoAssetBundle.LoadAsset(strAssetPath);
                    //判断类型
                    System.Type type = ms_dicUnityType2AssetType.HrTryGet(o.GetType()) as System.Type;
                    if (type != null)
                    {
                        res = Activator.CreateInstance(type, new object[] { strAssetPath, assetFile }) as HrResource;
                        if (res == null)
                        {
                            HrLogger.LogError("ActionAssetBundleLoadFinished Error! assetName:" + o.name + " AssetBundle:" + assetFile.Name);
                            if (loadResourceCallBack != null && loadResourceCallBack.LoadResourceFailed != null)
                            {
                                loadResourceCallBack.LoadResourceFailed(strAssetPath, "can not create the instance!");
                            }
                        }
                        else
                        {
                            m_dicItemResourceInfo.Add(strAssetPath, res);
                            if (loadResourceCallBack != null && loadResourceCallBack.LoadResourceSuccess != null)
                            {
                                loadResourceCallBack.LoadResourceSuccess(res);
                            }
                        }
                    }
                    else
                    {
                        HrLogger.LogError("ActionAssetBundleLoadFinished Error! Can not find the asset type:" + o.name);
                        if (loadResourceCallBack != null && loadResourceCallBack.LoadResourceFailed != null)
                        {
                            loadResourceCallBack.LoadResourceFailed(strAssetPath, "can not find the resource's type!");
                        }
                    }

                }
                else
                {
                    HrLogger.LogError(string.Format("load asset bundle {0} error! ", strAssetBundleName));
                    if (loadResourceCallBack != null && loadResourceCallBack.LoadResourceFailed != null)
                    {
                        loadResourceCallBack.LoadResourceFailed(strAssetPath, string.Format("load asset bundle {0} error! ", strAssetBundleName));
                    }
                }
            }
            else
            {
                if (loadResourceCallBack != null && loadResourceCallBack.LoadResourceSuccess != null)
                {
                    loadResourceCallBack.LoadResourceSuccess(res);
                }
            }
        }

        public void LoadResourceSync(string strResourceName, HrLoadResourceCallBack loadResourceCallBack)
        {
            int nID = 0;
            m_dicResourcePath2IDInfo.TryGetValue(strResourceName, out nID);
            if (nID > 0)
            {
                LoadResourceSync(nID, loadResourceCallBack);
            }
        }

        /// <summary>
        /// 手动加载AssetBundle
        /// </summary>
        /// <param name="strAssetBundle"></param>
        public void LoadAssetBundleSync(string strAssetBundleName, HrLoadAssetCallBack loadAssetCallBack)
        {
            HrAssetFile loadedAssetFile = m_dicAssetFileInfo.HrTryGet(strAssetBundleName);
            if (loadedAssetFile != null && loadedAssetFile is HrAssetBundle)
            {
                if (loadedAssetFile.IsLoaded())
                {
                    //loadAssetCallBack.LoadAssetSuccess?.Invoke(loadedAssetFile);
                    if (loadAssetCallBack != null && loadAssetCallBack.LoadAssetSuccess != null)
                        loadAssetCallBack.LoadAssetSuccess(loadedAssetFile);
                    return;
                }

                if (loadedAssetFile.IsLoading())
                {
                    HrLogger.LogWarning(string.Format("try to load assetbundle [{0}] sync, but find that the assetbundle is loading!", strAssetBundleName));
                    while (true)
                    {
                        if (loadedAssetFile.IsLoaded())
                        {
                            if (loadAssetCallBack != null && loadAssetCallBack.LoadAssetSuccess != null)
                                loadAssetCallBack.LoadAssetSuccess(loadedAssetFile);
                            return;
                        }
                        if (loadedAssetFile.IsError())
                        {
                            if (loadAssetCallBack != null && loadAssetCallBack.LoadAssetFailed != null)
                                loadAssetCallBack.LoadAssetFailed(strAssetBundleName, "unkonw error!");
                            return;
                        }
                    }
                }

                if (loadedAssetFile.IsError())
                {
                    throw new HrException(string.Format("the assetbundle '{0}' is loading or is error", strAssetBundleName));
                }

                loadedAssetFile.LoadSync();
                if (loadedAssetFile.IsLoaded())
                {
                    if (loadAssetCallBack != null && loadAssetCallBack.LoadAssetSuccess != null)
                    {
                        loadAssetCallBack.LoadAssetSuccess(loadedAssetFile);
                    }
                    return;
                }

                if (loadedAssetFile.IsError())
                {
                    if (loadAssetCallBack != null && loadAssetCallBack.LoadAssetFailed != null)
                        loadAssetCallBack.LoadAssetFailed(strAssetBundleName, "unkonw error!");

                    throw new HrException(string.Format("the assetbundle '{0}' is loading or is error", strAssetBundleName));
                }
            }
            else
            {
                if (loadAssetCallBack != null && loadAssetCallBack.LoadAssetFailed != null)
                    loadAssetCallBack.LoadAssetFailed(strAssetBundleName, "can not find the asset bundle or the asset bundle's type is not HrAssetBundle");
                return;
            }
        }

        public void LoadAssetBundleAsync(string strAssetBundleName, HrLoadAssetCallBack loadAssetCallBack)
        {
            HrAssetFile loadedAssetFile = m_dicAssetFileInfo.HrTryGet(strAssetBundleName);
            if (loadedAssetFile != null && loadedAssetFile is HrAssetBundle)
            {
                if (loadedAssetFile.IsLoaded())
                {
                    //loadAssetCallBack.LoadAssetSuccess?.Invoke(loadedAssetFile);
                    if (loadAssetCallBack.LoadAssetSuccess != null)
                        loadAssetCallBack.LoadAssetSuccess(loadedAssetFile);
                    return;    
                }

                if (loadedAssetFile.IsLoading())
                {
                    HrLogger.LogWarning(string.Format("try to load assetbundle [{0}] sync, but find that the assetbundle is loading!", strAssetBundleName));
                    while (true)
                    {
                        if (loadedAssetFile.IsLoaded())
                        {
                            if (loadAssetCallBack.LoadAssetSuccess != null)
                                loadAssetCallBack.LoadAssetSuccess(loadedAssetFile);
                            return;
                        }
                        if (loadedAssetFile.IsError())
                        {
                            if (loadAssetCallBack.LoadAssetFailed != null)
                                loadAssetCallBack.LoadAssetFailed(strAssetBundleName, "unkonw error!");
                            return;
                        }
                    }
                }

                if (loadedAssetFile.IsError())
                {
                    throw new HrException(string.Format("the assetbundle '{0}' is loading or is error", strAssetBundleName));
                }

                loadedAssetFile.LoadAsync(loadAssetCallBack);
                if (loadedAssetFile.IsLoaded())
                {
                    if (loadAssetCallBack.LoadAssetSuccess != null)
                    {
                        loadAssetCallBack.LoadAssetSuccess(loadedAssetFile);
                    }
                    return;
                }

                if (loadedAssetFile.IsError())
                {
                    if (loadAssetCallBack.LoadAssetFailed != null)
                        loadAssetCallBack.LoadAssetFailed(strAssetBundleName, "unkonw error!");

                    throw new HrException(string.Format("the assetbundle '{0}' is loading or is error", strAssetBundleName));
                }
            }
            else
            {
                if (loadAssetCallBack.LoadAssetFailed != null)
                    loadAssetCallBack.LoadAssetFailed(strAssetBundleName, "can not find the asset bundle or the asset bundle's type is not HrAssetBundle");
                return;
            }
        }

        public void LoadAssetBundleWithFullPathSync(string strFullPath, HrLoadAssetCallBack loadAssetCallBack)
        {
            char[] delimiterChars = { '\\', '/' };
            string[] strArr = strFullPath.Split(delimiterChars);
            string strName = strFullPath;
            if (strArr.Length > 1)
            {
                strName = strArr[strArr.Length - 1];
            }

            HrAssetFile assetFile = new HrAssetBundle(strName, strFullPath);
            assetFile.LoadSync();

            if (assetFile.IsLoaded())
            {
                if (loadAssetCallBack != null && loadAssetCallBack.LoadAssetSuccess != null)
                {
                    loadAssetCallBack.LoadAssetSuccess(assetFile);
                }
                else
                {
                    assetFile.Release();
                }
            }
        }

        #region private methods

        private void LoadDataTableConfig()
        {
            string strDataTableConfigFilePath = HrResourcePath.GetDataTableConfigFilePath();
            if (!File.Exists(strDataTableConfigFilePath))
            {
                HrLogger.LogError(string.Format("can not find the datatable config file [{0}]", strDataTableConfigFilePath));
                return;
            }

            string strData = File.ReadAllText(strDataTableConfigFilePath);
            JsonData jsonData = JsonMapper.ToObject(strData);
            IDictionary dicJsonData = jsonData as IDictionary;
            if (dicJsonData == null)
            {
                HrLogger.LogError(string.Format("parse json data error! file [{0}]", strDataTableConfigFilePath));
                return;
            }

            int nDataTableIndex = 0;
            while (true)
            {
                string strDataTableItem = string.Format("SheetName_{0}", nDataTableIndex);
                if (dicJsonData.Contains(strDataTableItem))
                {
                    string strBinaryFileName = jsonData[strDataTableItem]["Name"].ToString();
                    if (m_dicAssetFileInfo.ContainsKey(strBinaryFileName))
                    {
                        HrLogger.LogWarning(string.Format("the binaryfile whose  name is '{0}' is existed!", strBinaryFileName));

                        ++nDataTableIndex;
                        continue;
                    }
                    string strBinaryFileFullPath = HrResourcePath.CombineDataTablePath(strBinaryFileName);
                    var assetBinary = CreateAssetBinary(strBinaryFileName, strBinaryFileFullPath);

                    JsonData sheetNames = jsonData[strDataTableItem]["SheetName"];
                    if (sheetNames.IsArray)
                    {
                        for (int i = 0; i < sheetNames.Count; ++i)
                        {
                            string strSheetName = sheetNames[i].ToString();
                            m_dicDataTableInBinaryInfo.Add(strSheetName, strBinaryFileName);
                            assetBinary.DataTables.Add(strSheetName);
                        }
                    }
                }
                else
                {
                    break;
                }
                ++nDataTableIndex;
            }
        }

        /// <summary>
        /// 解析资源配置文件，创建对应资源
        /// </summary>
        private void LoadAssetsListConfig()
        {
            string strAssetsListConfigFilePath = HrResourcePath.GetAssetBundleAssetsListFilePath();
            if (!File.Exists(strAssetsListConfigFilePath))
            {
                HrLogger.LogError(string.Format("can not find the assets list config file [{0}]", strAssetsListConfigFilePath));
                return;
            }
            string strData = File.ReadAllText(strAssetsListConfigFilePath);
            JsonData jsonData = JsonMapper.ToObject(strData);
            IDictionary dicJsonData = jsonData as IDictionary;
            if (dicJsonData == null)
            {
                HrLogger.LogError(string.Format("parse json data error! file [{0}]", strAssetsListConfigFilePath));
                return;
            }

            //解析AssetBundle配置信息
            int nAssetBundleIndex = 0;
            while (true)
            {
                string strAssetBundleItem = string.Format("AssetBundle_{0}", nAssetBundleIndex);
                if (dicJsonData.Contains(strAssetBundleItem))
                {
                    string strAssetBundleName = jsonData[strAssetBundleItem]["AssetBundle"].ToString();
                    if (m_dicAssetFileInfo.ContainsKey(strAssetBundleName))
                    {
                        HrLogger.LogWarning(string.Format("the assetbudlefile whose name is '{0}' is existed!", strAssetBundleName));

                        ++nAssetBundleIndex;
                        continue;
                    }
                    string strAssetBundleFullPath = HrResourcePath.CombineAssetBundlePath(strAssetBundleName);

                    var assetBundle = CreateAssetBundle(strAssetBundleName, strAssetBundleFullPath);

                    JsonData dependencies = jsonData[strAssetBundleItem]["Dependencies"];
                    if (dependencies.IsArray)
                    {
                        for (var i = 0; i < dependencies.Count; ++i)
                        {
                            assetBundle.AsssetDependicesInfo.Add(dependencies[i].ToString());
                        }
                    }
                    JsonData beDependent = jsonData[strAssetBundleItem]["Bedependent"];
                    if (beDependent.IsArray)
                    {
                        for (var i = 0; i < beDependent.Count; ++i)
                        {
                            assetBundle.BeDependentOnAssetInfo.Add(beDependent[i].ToString());
                        }
                    }

                }
                else
                {
                    break;
                }
                ++nAssetBundleIndex;
            }

            //解析Resource配置信息
            int nAssetIndex = 0;
            while (true)
            {
                string strAssetItem = string.Format("Asset_{0}", nAssetIndex);
                if (dicJsonData.Contains(strAssetItem))
                {
                    int nID = (int)jsonData[strAssetItem]["ID"];
                    string strFilePath = jsonData[strAssetItem]["FilePath"].ToString().ToLower();
                    string strAssetBundleName = jsonData[strAssetItem]["AssetBundle"].ToString();

                    m_dicResourceID2PathAndAssetBundle.Add(nID, new KeyValuePair<string, string>(strFilePath, strAssetBundleName));
                    m_dicResourcePath2IDInfo.Add(strFilePath, nID);
                }
                else
                {
                    break;
                }
                ++nAssetIndex;
            }
        }

        private HrAssetBinary CreateAssetBinary(string strName, string strFullPath)
        {
            HrAssetBinary assetBinary = new HrAssetBinary(strName, strFullPath);
            assetBinary.LoadAssetBundleEvent.LoadAssetSuccessHandler += LoadAssetBinarySuccessHandler;
            assetBinary.LoadAssetBundleEvent.LoadAssetProgressHandler += LoadAssetBinaryProgressHandler;
            assetBinary.LoadAssetBundleEvent.LoadAssetFailedHandler += LoadAssetBinaryFailedHandler;

            m_dicAssetFileInfo.Add(strName, assetBinary);

            return assetBinary;
        }

        private HrAssetBundle CreateAssetBundle(string strName, string strFullPath)
        {
            HrAssetBundle assetBundle = new HrAssetBundle(strName, strFullPath);
            assetBundle.LoadAssetBundleEvent.LoadAssetSuccessHandler += LoadAssetBundleSuccessHandler;
            assetBundle.LoadAssetBundleEvent.LoadAssetFailedHandler += LoadAssetBundleFailedHandler;
            assetBundle.LoadAssetBundleEvent.LoadAssetProgressHandler += LoadAssetBundleProgressHandler;

            m_dicAssetFileInfo.Add(strName, assetBundle);

            return assetBundle;
        }

        private void LoadAssetBinarySuccessHandler(object sender, EventArgs args)
        {
            HrLoadAssetSuccessEventArgs eventArgs = args as HrLoadAssetSuccessEventArgs;
            HrLogger.Log(string.Format("LoadAssetBundleSuccess! assetBundle:{0} duration:{1}", eventArgs.AssetName, eventArgs.Duration));
            HrAssetBinary assetBinary = eventArgs.UserData as HrAssetBinary;

            ParseBinaryFile(assetBinary);
        }

        private void LoadAssetBinaryFailedHandler(object sender, EventArgs args)
        {

        }

        private void LoadAssetBinaryProgressHandler(object sender, EventArgs args)
        {

        }

        private void ParseBinaryFile(HrAssetBinary assetBinary)
        {
            for (var i = 0; i < assetBinary.DataTables.Count; ++i)
            {
                string strSheetName = assetBinary.DataTables[i];
                HrResourceBinary resBinary = new HrResourceBinary(strSheetName, assetBinary);
                m_dicItemResourceInfo.Add(strSheetName, resBinary);
            }
        }

        private void LoadAssetBundleSuccessHandler(object sender, EventArgs args)
        {
            HrLoadAssetSuccessEventArgs eventArgs = args as HrLoadAssetSuccessEventArgs;
            HrLogger.Log(string.Format("LoadAssetBundleSuccess! assetBundle:{0} duration:{1}", eventArgs.AssetName,  eventArgs.Duration));
            //HrAssetFile assetBundle = eventArgs.UserData as HrAssetFile;

            //ParseAssetBundle(assetBundle as HrAssetBundle);
        }

        private void LoadAssetBundleFailedHandler(object sender, EventArgs args)
        {

        }

        private void LoadAssetBundleProgressHandler(object sender, EventArgs args)
        {

        }

        //private void ParseAssetBundle(HrAssetBundle assetBundle)
        //{
        //    var strAllAssetsNameArr = assetBundle.MonoAssetBundle.GetAllAssetNames();
        //    foreach (var strAssetName in strAllAssetsNameArr)
        //    {
        //        var o = assetBundle.MonoAssetBundle.LoadAsset(strAssetName);
        //        //判断类型
        //        System.Type type = ms_dicUnityType2AssetType.HrTryGet(o.GetType()) as System.Type;
        //        if (type != null)
        //        {
        //            HrResource res = Activator.CreateInstance(type, new object[] { strAssetName, assetBundle }) as HrResource;
        //            if (res == null)
        //            {
        //                HrLogger.LogError("ActionAssetBundleLoadFinished Error! assetName:" + o.name + " AssetBundle:" + assetBundle.Name);
        //            }
        //            else
        //            {
        //                m_dicItemResourceInfo.Add(strAssetName, res);
        //            }
        //        }
        //        else
        //        {
        //            HrLogger.LogError("ActionAssetBundleLoadFinished Error! Can not find the asset type:" + o.name);
        //        }
        //    }
        //}

        #endregion
    }
}

