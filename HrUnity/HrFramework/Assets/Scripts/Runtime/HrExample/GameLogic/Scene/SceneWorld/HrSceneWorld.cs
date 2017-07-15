using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Scene
{
    public class HrSceneWorld : HrScene
    {
        public HrSceneWorld(HrSceneManager sceneManager) :base(sceneManager)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            HrLogger.Log("HrSceneWorld OnEnter!");

            HrGameWorld.Instance.UIComponent.Clear();
            HrGameWorld.Instance.SceneComponent.UnloadCurrentScene();
            //这一帧其实并没有切换成功，所以应该放在下一帧执行逻辑,同步加载会造成误解
            HrGameWorld.Instance.SceneComponent.LoadCachedSceneSync();

            ChangeState<Procedure.HrSceneWorld.HrProcedureInit>();
        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {

        }

        public override void OnExit()
        {

        }

        public override void OnDestroy()
        {

        }

        protected override void AddProcedure()
        {
            m_fsmProcedureStateMachine.AddState(new Procedure.HrSceneWorld.HrProcedureInit(this));
            m_fsmProcedureStateMachine.AddState(new Procedure.HrSceneWorld.HrProcedureMain(this));

        }
    }

}
