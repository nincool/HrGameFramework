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

            //加载配置数据
            LoadDataTables();
        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
            base.OnUpdate(fElapseSeconds, fRealElapseSeconds);

            HrGameWorld.Instance.SceneComponent.SwitchToScene<Hr.Scene.HrSceneBattlePreload>();
        }

        public override void OnExit()
        {
            base.OnExit();

        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void LoadDataTables()
        {
            //1.加载显示质量配置
            HrGameWorld.Instance.DataTableComponent.LoadDataTable(HrDataTableDeviceQuality.DataTableDefaultName);
            //2.加载预加载资源配置
            HrGameWorld.Instance.DataTableComponent.LoadDataTable(HrDataTablePreloadWorldScene.DataTableDefaultName);
            HrGameWorld.Instance.DataTableComponent.LoadDataTable(HrDataTablePreloadBattleScene.DataTableDefaultName);
        }
    }
}
