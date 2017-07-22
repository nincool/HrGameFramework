using Hr.DataTable;
using Hr.EventSystem;
using Hr.Resource;
using Hr.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hr.Scene.Procedure.HrSceneLaunch
{
    public class HrProcedureReadyEnterGame : HrProcedure
    {
        public HrProcedureReadyEnterGame(HrScene owner) : base(owner)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            HrLogger.Log("HrSceneLaunch.ProcedureReadyEnterGame.OnEnter!");
           
            //加载资源配置 这个放在动态更新之前
            HrGameWorld.Instance.ResourceComponent.LoadAssetsConfig();
        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
            base.OnUpdate(fElapseSeconds, fRealElapseSeconds);

            HrGameWorld.Instance.SceneComponent.SwitchToScene<Hr.Scene.HrSceneWorldPreload>();
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
