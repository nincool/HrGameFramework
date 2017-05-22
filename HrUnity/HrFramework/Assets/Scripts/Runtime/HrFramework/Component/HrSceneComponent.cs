using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public class HrSceneComponent : HrComponent
    {
        public bool InitSuccess { get; private set; }

        private HrSceneManager m_sceneManager = null;

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
            return m_sceneManager.CurrentScene.GetType().FullName;
        }
    }

}
