using Hr.DataTable;
using Hr.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hr.Scene.Procedure.HrSceneLaunch
{
    public class HrProcedurePreload : HrProcedure
    {
        private List<int> m_lisPreloadResourceID = new List<int>();

        private HrLoadResourceCallBack m_loadResourceCallBack;

        public HrProcedurePreload(HrScene owner) : base(owner)
        {
            m_loadResourceCallBack = new HrLoadResourceCallBack(LoadResourceSuccess, LoadResourceFailed);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            HrLogger.Log("HrSceneLaunch.HrProcedurePreload.OnEnter!");

            PreloadResources();

            InitGameSettingConfigs();
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
            //加载资源配置 这个放在动态更新之前
            HrGameWorld.Instance.ResourceComponent.LoadAssetsConfig();

            //加载配置数据
            LoadDataTables();

            //加载AssetBundle
            LoadAssetBundles();
        }
        
        private void LoadDataTables()
        {
            //1.加载显示质量配置
            HrGameWorld.Instance.DataTableComponent.LoadDataTable(HrDataTableDeviceQuality.DataTableDefaultName);
            //2.加载预加载资源配置
            HrGameWorld.Instance.DataTableComponent.LoadDataTable(HrDataTablePreloadWorldScene.DataTableDefaultName);
        }

        private void LoadAssetBundles()
        {
            //1.根据配置文件获取Scene中需要自动加载的AssetBundle
            //2.有些动态资源比如RPG中不同角色的不同资源根据实际需求加载
            //m_lisReadLoadAssetBundle.Add();
            var dataTable = HrGameWorld.Instance.DataTableComponent.GetDataTable(HrDataTablePreloadWorldScene.DataTableDefaultName) as HrDataTablePreloadWorldScene;
            if (dataTable != null)
            {
                foreach (var itemPreloadResouceInfo in dataTable.PreloadWorldSceneInfo)
                {
                    m_lisPreloadResourceID.Add(itemPreloadResouceInfo.Value.ResourceID);
                } 
            }
            else
            {
                throw new HrException("can not find the info of preload res");
            }
            HrCoroutineManager.StartCoroutine(StartLoadAssetBundles());
        }

        private IEnumerator StartLoadAssetBundles()
        {
            foreach (var nResourceID in m_lisPreloadResourceID)
            {
                HrGameWorld.Instance.ResourceComponent.LoadResourceSync(nResourceID, m_loadResourceCallBack);
                yield return null;
            }
            yield return null;

            OnPreloadFinished();
        }

        private void InitGameSettingConfigs()
        {
            InitQualitySettingConfig();
        }

        private void InitQualitySettingConfig()
        {
            var dataTable = HrGameWorld.Instance.DataTableComponent.GetDataTable(HrDataTableDeviceQuality.DataTableDefaultName) as HrDataTableDeviceQuality;
            if (dataTable != null)
            {
                QualitySettings.SetQualityLevel(dataTable.GetDefalutQualityLevel());
            }
        }

        private void LoadResourceSuccess(HrResource res)
        {
            HrLogger.Log(string.Format("load res '{0}' success", res.AssetName));
        }

        private void LoadResourceFailed(string strResourceName, string strErrorMsg)
        {
            HrLogger.LogError(string.Format("load res '{0}' error! error msg '{1}'", strResourceName, strErrorMsg));
        }

        private void OnPreloadFinished()
        {
            HrGameWorld.Instance.SceneComponent.SwitchToScene<Hr.Scene.HrSceneWorld>();
        }
    }
}
