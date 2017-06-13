using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hr.Scene.Procedure.HrSceneLaunch
{
    public class HrProcedurePreload : HrProcedure
    {
        public HrProcedurePreload(HrScene owner) : base(owner)
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();
            HrLogger.Log("HrSceneLaunch.HrProcedurePreload.OnEnter!");

        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
            base.OnUpdate(fElapseSeconds, fRealElapseSeconds);

        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void PreloadResources()
        {

        }
        
        private void LoadDataTables()
        {

        }
    }
}
