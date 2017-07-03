using System;
using System.Collections.Generic;
using Hr;
using Hr.Resource;
using UnityEngine.SceneManagement;

namespace Hr.Scene
{

    public class HrSceneManager : HrModule, ISceneManager
    {
        private Dictionary<string, HrScene> m_dicScene = new Dictionary<string, HrScene>();

        private IFSMStateMachine m_fsmSceneStateMachine = null;

        private HrLoadResourceCallBack m_loadSceneCallBack = null;

        private HrLoadAssetCallBack m_loadSceneAssetBundleCallBack = null;

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
        }

        private void LoadSceneFailed(string strSceneName, string strErrorMsg)
        {
            HrLogger.LogError(string.Format("load scene error! msg '{0}'", strErrorMsg));
        }

        private void LoadSceneAssetBundleSuccess(HrAssetFile assetFile)
        {
            HrLogger.Log(string.Format("load scene assetbundle '{0}' success!", assetFile.Name));
            HrAssetBundle assetBundle = assetFile as HrAssetBundle;
            if (assetBundle != null)
            {
                var scenePaths = assetBundle.MonoAssetBundle.GetAllScenePaths();
                if (scenePaths.Length > 0)
                {
                    SceneManager.LoadScene(scenePaths[0], LoadSceneMode.Additive);
                }
                assetBundle.AutoRelease();
            }
        }

        private void LoadSceneAssetBundleFailed(string strAssetBundle, string strErrorMsg)
        {

        }
        #endregion
    }
}
