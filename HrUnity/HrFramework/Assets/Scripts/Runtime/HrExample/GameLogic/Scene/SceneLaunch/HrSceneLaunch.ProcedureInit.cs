using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Scene.Procedure.HrSceneLaunch
{
    public class HrProcedureInit : HrProcedure
    {
        public HrProcedureInit(HrScene owner) : base(owner)
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();
            HrLogger.Log("HrSceneLaunch.HrProcedureInit.OnEnter!");

            InitAppSetting();
        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
            base.OnUpdate(fElapseSeconds, fRealElapseSeconds);

            m_owner.ChangeState<HrProcedureSplash>();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void InitAppSetting()
        {
            ///TODO 设置 音量
        }

    }
}
