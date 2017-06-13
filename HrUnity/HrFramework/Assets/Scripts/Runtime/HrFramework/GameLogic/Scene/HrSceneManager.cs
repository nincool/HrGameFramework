using System;
using System.Collections.Generic;
using Hr;

namespace Hr.Scene
{

    public class HrSceneManager : HrModule, ISceneManager
    {
        private Dictionary<string, HrScene> m_dicScene = new Dictionary<string, HrScene>();

        private IFSMStateMachine m_fsmSceneStateMachine = null;

        public HrSceneManager()
        {
        }

        public override void Init()
        {
            m_fsmSceneStateMachine = HrGameWorld.Instance.FSMComponent.AddFSM<HrSceneManager>(typeof(HrSceneManager).FullName, this) as HrFSMStateMachine<HrSceneManager>;
        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {

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
    }
}
