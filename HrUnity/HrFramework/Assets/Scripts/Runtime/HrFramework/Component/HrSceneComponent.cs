using Hr.EventSystem;
using Hr.Resource;
using Hr.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hr
{
    public sealed class HrSceneComponent : HrComponent
    {
        public bool InitSuccess { get; private set; }

        private ISceneManager m_sceneManager = null;

        private IEventManager m_eventManger = null;
        /// <summary>
        /// 资源管理器
        /// </summary>
        private IResourceManager m_resourceManager = null;

        /// <summary>
        /// 缓存加载成功的Resource Scene
        /// </summary>
        private HrResourceScene m_cachedResourceScene = null;

        /// <summary>
        /// 缓存的加载成功的AssetBundle
        /// </summary>
        private HrAssetBundle m_cachedSceneAssetFile = null;

        /// <summary>
        /// 加载单个SceneResource回调
        /// </summary>
        private HrLoadResourceCallBack m_loadSceneCallBack = null;

        /// <summary>
        /// 加载存有Scene的AssetBundle回调
        /// </summary>
        private HrLoadAssetCallBack m_loadSceneAssetBundleCallBack = null;

        /// <summary>
        /// Unity Scene
        /// </summary>
        private UnityEngine.SceneManagement.Scene m_currentUnityScene;

        protected override void Awake()
        {
            base.Awake();

            m_sceneManager = HrGameWorld.Instance.GetModule<ISceneManager>();
            m_eventManger = HrGameWorld.Instance.GetModule<IEventManager>();
            m_resourceManager = HrGameWorld.Instance.GetModule<IResourceManager>();
            if (m_sceneManager != null && m_eventManger != null && m_resourceManager != null)
            {
                InitSuccess = true;
            }

            m_loadSceneCallBack = new HrLoadResourceCallBack(LoadSceneSuccess, LoadSceneFailed);
            m_loadSceneAssetBundleCallBack = new HrLoadAssetCallBack(LoadSceneAssetBundleSuccess, LoadSceneAssetBundleFailed);
        }

        protected override void Start()
        {
            base.Start();
        }

        public void LoadUnitySceneAssetBundleSync(string strAssetBundleFullPath)
        {
            m_resourceManager.LoadAssetBundleWithFullPathSync(strAssetBundleFullPath, m_loadSceneAssetBundleCallBack);
        }

        public void LoadUnitySceneAssetBundleAsync(string strAssetBundleFullPath)
        {
            StartCoroutine(LoadSceneAssetBundleAsync(strAssetBundleFullPath));
        }

        public void LoadUnitySceneSync(int nResID)
        {
            m_resourceManager.LoadResourceSync(nResID, m_loadSceneCallBack);
        }

        public void LoadUnitySceneAsync(int nResID)
        {
            StartCoroutine(LoadUnitySceneAsyncCoroutine(nResID));
        }

        public void LoadUnityCachedSceneSync()
        {
            if (m_cachedSceneAssetFile != null && m_cachedSceneAssetFile.IsLoaded())
            {
                var scenePaths = m_cachedSceneAssetFile.MonoAssetBundle.GetAllScenePaths();
                if (scenePaths.Length > 0)
                {
                    SceneManager.LoadScene(scenePaths[0], LoadSceneMode.Additive);
                    m_currentUnityScene = SceneManager.GetSceneByPath(scenePaths[0]);
                }
                m_cachedSceneAssetFile.AutoRelease();
                m_cachedSceneAssetFile = null;
            }
            else if (m_cachedResourceScene != null)
            {
                if (m_cachedResourceScene.RefAssetFile != null && m_cachedResourceScene.RefAssetFile.IsLoaded())
                {
                    HrAssetBundle assetBundle = m_cachedResourceScene.RefAssetFile as HrAssetBundle;
                    var scenePaths = assetBundle.MonoAssetBundle.GetAllScenePaths();
                    if (scenePaths.Length > 0)
                    {
                        SceneManager.LoadScene(scenePaths[0], LoadSceneMode.Additive);
                        m_currentUnityScene = SceneManager.GetSceneByPath(scenePaths[0]);
                    }
                    m_cachedResourceScene.RefAssetFile.AutoRelease();
                    m_cachedResourceScene = null;
                }
            }
        }

        public void UnloadUnityCurrentScene()
        {
            m_eventManger.SendEvent(this, new HrEventHandlerArgs(HrEventType.EVENT_SCENE_UNLOAD_SCENE, m_sceneManager.GetRunningScene().GetType()));

            if (m_currentUnityScene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(m_currentUnityScene);
            }
            else
            {
                HrLogger.LogWarning(string.Format("warning! try to unload current scene, but the scene '{0}' is not loaded!", m_currentUnityScene.path));
            }
        } 

        public void AddScene<T>()
        {
            AddScene(typeof(T).FullName);
        }

        public void AddScene(string strSceneType)
        {
            m_sceneManager.AddScene(strSceneType);
        }

        public void SwitchToScene<T>()
        {
            UnloadUnityCurrentScene();

            m_sceneManager.SwitchToScene(typeof(T).FullName);
        }

        public void SwitchToScene(string strSceneName)
        {
            m_sceneManager.SwitchToScene(strSceneName);
        }

        public string GetRunningSceneName()
        {
            return m_sceneManager.GetRunningScene().GetType().FullName;
        }

        public bool IsCurrentUnitySceneLoaded()
        {
            if (m_currentUnityScene != null)
            {
                return m_currentUnityScene.isLoaded;
            }

            return false;
        }

        #region private methods

        private void LoadSceneSuccess(HrResource resScene)
        {
            HrLogger.Log(string.Format("load scene success! name '{0}'", resScene.AssetName));
            m_cachedResourceScene = resScene as HrResourceScene;
            if (m_cachedResourceScene == null)
            {
                throw new HrException(string.Format("load scene success ! but the resource is null!"));
            }
            HrGameWorld.Instance.EventComponent.SendEvent(this, new HrEventLoadSceneResourceEventHandler(HrEventType.EVENT_LOAD_SCENE_RESOURCE_SUCCESS, null));
        }

        private void LoadSceneFailed(string strSceneName, string strErrorMsg)
        {
            HrLogger.LogError(string.Format("load scene error! msg '{0}'", strErrorMsg));
        }

        private void LoadSceneAssetBundleSuccess(HrAssetFile assetFile)
        {
            HrLogger.Log(string.Format("load scene assetbundle '{0}' success!", assetFile.Name));

            m_cachedSceneAssetFile = assetFile as HrAssetBundle;
            m_eventManger.SendEvent(this, new HrEventLoadSceneResourceEventHandler(HrEventType.EVENT_LOAD_SCENE_RESOURCE_SUCCESS, null));
        }

        private void LoadSceneAssetBundleFailed(string strAssetBundle, string strErrorMsg)
        {
            HrLogger.Log(string.Format("load scene assetbundle '{0}' failed! ErrorMsg '{1}'", strAssetBundle, strErrorMsg));
        }

        private IEnumerator LoadSceneAssetBundleAsync(string strAssetBundleFullPath)
        {
            IEnumerator itor = m_resourceManager.LoadAssetBundleWithFullPathAsync(strAssetBundleFullPath, m_loadSceneAssetBundleCallBack);
            while (itor.MoveNext())
            {
                yield return null;
            }
        }

        private IEnumerator LoadUnitySceneAsyncCoroutine(int nResID)
        {
            IEnumerator itor = m_resourceManager.LoadResourceAsync(nResID, m_loadSceneCallBack);
            while (itor.MoveNext())
            {
                yield return null;
            }
        }

        #endregion
    }
}



