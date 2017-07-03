using Hr.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hr
{
    public class HrSceneComponent : HrComponent
    {
        public bool InitSuccess { get; private set; }

        private ISceneManager m_sceneManager = null;

        protected override void Awake()
        {
            base.Awake();

            m_sceneManager = HrGameWorld.Instance.GetModule<HrSceneManager>();
            if (m_sceneManager != null)
            {
                InitSuccess = true;
            }
        }

        protected override void Start()
        {
            base.Start();
        }

        public void LoadSceneSync(string strAssetBundleFullPath)
        {
            m_sceneManager.LoadSceneAssetBundleSync(strAssetBundleFullPath);
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
    }
}



