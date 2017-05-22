using Hr;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Hr
{

    public class HrSceneManager : HrModule
    {
        private Dictionary<string, HrScene> m_dicScene = new Dictionary<string, HrScene>();

        private HrScene m_runningScene;

        private HrFSMStateMachine<HrSceneManager> m_fsmSceneStateMachine = null;

        public HrScene CurrentScene
        {
            get { return m_runningScene; }
        }

        public HrSceneManager()
        {
        }

        public override void Init()
        {
            m_dicScene.Add(typeof(HrLaunchScene).FullName, new HrLaunchScene());

            m_fsmSceneStateMachine = HrGameWorld.Instance.FSMComponent.CreateFSM<HrSceneManager>(typeof(HrSceneManager).FullName, this) as HrFSMStateMachine<HrSceneManager>;
            m_fsmSceneStateMachine.AddState(new HrLaunchScene());
        }

        public override void Update(float fElapseSeconds, float fRealElapseSeconds)
        {

        }

        /// <summary>
        /// 在unity切换场景成功之后调用
        /// </summary>
        /// <param name="sceneType"></param>
        public void SwitchToScene(string strSceneType)
        {
            var readyToScene = m_dicScene.HrTryGet(strSceneType);
            if (readyToScene == null)
            {
                HrLogger.LogError("HrSceneManager SwitchToScene Error! SceneType:" + strSceneType);
                return;
            }
            m_fsmSceneStateMachine.ChangeState(Type.GetType(strSceneType));
        }
    }
}
