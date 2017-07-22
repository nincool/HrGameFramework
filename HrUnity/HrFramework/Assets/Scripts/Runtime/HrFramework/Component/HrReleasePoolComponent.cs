using Hr.EventSystem;
using Hr.ReleasePool;
using Hr.Scene;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public class HrReleasePoolComponent : HrComponent
    {
        public bool InitSuccess { get; private set; }

        private IReleasePoolManager m_releasePoolManager;

        private IEventManager m_eventManager;

        protected override void Awake()
        {
            base.Awake();

            m_releasePoolManager = HrGameWorld.Instance.GetModule<IReleasePoolManager>();
            m_eventManager = HrGameWorld.Instance.GetModule<IEventManager>();
            if (m_releasePoolManager != null && m_eventManager != null)
            {
                InitSuccess = true;
            }

            m_eventManager.AddHandler(HrEventType.EVENT_SCENE_LOADED_SCENE, HandleLoadedUnityScene);
            m_eventManager.AddHandler(HrEventType.EVENT_SCENE_UNLOAD_SCENE, HandleUnloadUnityScene);
        }

        public void AddReleaseStrategyObject(HrReleaseStrategy ReleaseStrategy)
        {
            m_releasePoolManager.AddObject(ReleaseStrategy);
        }

        #region private methods

        private void HandleLoadedUnityScene(object sender, HrEventHandlerArgs args)
        {
        }

        private void HandleUnloadUnityScene(object sender, HrEventHandlerArgs args)
        {
            Type t = args.UserData as Type;
            if (!t.IsSubclassOf(typeof(HrScenePreload)))
            {
                m_releasePoolManager.OnChangeScene();
            }
        }

        #endregion
    }
}
