using com.ootii.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hr.Scene.Procedure.HrSceneThirdPersonController
{
    public class HrProcedureInit : HrProcedure
    {

        public HrProcedureInit(HrScene owner) : base(owner)
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();
            HrLogger.Log("HrSceneThirdPersonController.HrProcedureInit.OnEnter!");

            HrGameWorld.Instance.InputComponent.AddAlias("Jump", EnumInput.SPACE);
        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
            base.OnUpdate(fElapseSeconds, fRealElapseSeconds);

            //m_owner.ChangeState<HrProcedureSplash>();
            //m_owner.ChangeState<HrProcedureTest>();


        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }



    }
}
