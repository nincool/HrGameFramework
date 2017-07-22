using Hr.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Scene.Procedure.HrSceneLaunch
{
    public class HrProcedureSplash : HrProcedure
    {
        public HrProcedureSplash(HrScene owner) : base(owner)
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();
            HrLogger.Log("HrSceneLaunch.HrProcedureSplash.OnEnter!");
        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
            base.OnUpdate(fElapseSeconds, fRealElapseSeconds);


            if (HrEnvironment.IsEditorMode)
            {
                m_owner.ChangeState<HrProcedureReadyEnterGame>();
            }
            else
            {
                ///TODO 资源解压缩
                m_owner.ChangeState<HrProcedureCheckVersion>();
            }
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
