using System;
using System.Collections.Generic;
using Hr;
using Hr.Resource;
using UnityEngine.SceneManagement;
using Hr.EventSystem;

namespace Hr.Scene
{

    public class HrSceneManager : HrModule, ISceneManager
    {
        private Dictionary<string, HrScene> m_dicScene = new Dictionary<string, HrScene>();

        private IFSMStateMachine m_fsmSceneStateMachine = null;


        /// <summary>
        /// 加载单个SceneResource回调
        /// </summary>
        private HrLoadResourceCallBack m_loadSceneCallBack = null;

        /// <summary>
        /// 加载存有Scene的AssetBundle回调
        /// </summary>
        private HrLoadAssetCallBack m_loadSceneAssetBundleCallBack = null;

        /// <summary>
        /// 缓存加载成功的Resource Scene
        /// </summary>
        private HrResourceScene m_cachedResourceScene = null;
        
        /// <summary>
        /// 缓存的加载成功的AssetBundle
        /// </summary>
        private HrAssetBundle m_cachedSceneAssetFile = null;

        private UnityEngine.SceneManagement.Scene m_currentUnityScene;

        public HrSceneManager()
        {
            m_loadSceneCallBack = new HrLoadResourceCallBack(LoadSceneSuccess, LoadSceneFailed);
            m_loadSceneAssetBundleCallBack = new HrLoadAssetCallBack(LoadSceneAssetBundleSuccess, LoadSceneAssetBundleFailed);
        }

        public override void Init()
        {
            m_fsmSceneStateMachine = HrGameWorld.Instance.FSMComponent.AddFSM<HrSceneManager>(typeof(HrSceneManager).FullName, this) as HrFSMStateMachine<HrSceneManager>;
        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {

        }

        public void LoadSceneAssetBundleSync(string strAssetBundleFullPath)
        {
            HrGameWorld.Instance.ResourceComponent.LoadAssetBundleWithFullPathSync(strAssetBundleFullPath, m_loadSceneAssetBundleCallBack);
        }

        public void LoadSceneSync(int nResID)
        {
            HrGameWorld.Instance.ResourceComponent.LoadResourceSync(nResID, m_loadSceneCallBack);
        }

        public void LoadCachedSceneSync()
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

        public void UnloadCurrentScene()
        {
            if (m_currentUnityScene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(m_currentUnityScene);
            }
            else
            {
                HrLogger.LogWarning(string.Format("warning! try to unload current scene, but the scene '{0}' is not loaded!", m_currentUnityScene.path));
            }
        }

        public HrScene GetRunningScene()
        {
            HrScene scene = m_fsmSceneStateMachine.GetCurrentState() as HrScene;
            return scene;
        }

        public void AddScene(string strSceneType)
        {
            Type sceneType = Type.GetType(strSceneType);
            if (sceneType == null)
            {
                HrLogger.LogError(string.Format("add scene error! scenetype:{0}", strSceneType));
                return;
            }

            var readyToScene = m_fsmSceneStateMachine.GetState(sceneType);
            if (readyToScene == null)
            {
                HrScene scene = (HrScene)Activator.CreateInstance(sceneType, this);
                m_fsmSceneStateMachine.AddState(scene);
            }
        }

        /// <summary>
        /// 在unity切换场景成功之后调用
        /// </summary>
        /// <param name="sceneType"></param>
        public void SwitchToScene(string strSceneType)
        {
            Type sceneType = Type.GetType(strSceneType);
            if (sceneType == null)
            {
                HrLogger.LogError(string.Format("SwitchToScene to error! scenetype:{0}", strSceneType));
                return;
            }

            m_fsmSceneStateMachine.ChangeState(sceneType);
        }

        public override void Shutdown()
        {

        }

        #region private fields
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
            if (m_cachedSceneAssetFile == null)
            {
                throw new HrException(string.Format("Load scene asset bundle success! but the assetfile'{0}' is null", assetFile.Name));
            }
            HrGameWorld.Instance.EventComponent.SendEvent(this, new HrEventLoadSceneResourceEventHandler(HrEventType.EVENT_LOAD_SCENE_RESOURCE_SUCCESS, null));
        }

        private void LoadSceneAssetBundleFailed(string strAssetBundle, string strErrorMsg)
        {

        }

        #endregion
    }
}
