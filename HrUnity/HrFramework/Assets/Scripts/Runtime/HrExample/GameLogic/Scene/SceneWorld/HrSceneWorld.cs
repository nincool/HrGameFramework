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
