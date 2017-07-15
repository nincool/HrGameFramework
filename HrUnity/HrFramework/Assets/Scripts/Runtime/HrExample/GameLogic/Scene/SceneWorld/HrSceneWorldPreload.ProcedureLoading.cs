using Hr.DataTable;
using Hr.EventSystem;
using Hr.Resource;
using Hr.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Scene.Procedure.HrSceneWorldPreload
{
    public class HrProcedureLoading : HrProcedure
    {
        private List<int> m_lisPreloadResourceID = new List<int>();
        private int m_nPreloadSceneResID = -1;
        private List<string> m_lisLoadedResource = new List<string>();

        private bool m_bNextScenePrepared = false;

        private HrLoadResourceCallBack m_loadResourceCallBack;

        public HrProcedureLoading(HrScene owner) : base(owner)
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
            HrGameWorld.Instance.DataTableComponent.LoadDataTable(HrDataTablePreloadBattleScene.DataTableDefaultName);
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
                    if (HrFileUtil.GetFileSuffix(itemPreloadResouceInfo.Value.ResourceName).Equals("unity"))
                    {
                        if (m_nPreloadSceneResID > 0)
                        {
                            throw new HrException("Ready to preload res error! multiply scene res!");
                        }
                        m_nPreloadSceneResID = itemPreloadResouceInfo.Value.ResourceID;
                    }
                    else
                    {
                        m_lisPreloadResourceID.Add(itemPreloadResouceInfo.Value.ResourceID);
                    }
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

            if (m_nPreloadSceneResID > 0)
            {
                HrGameWorld.Instance.EventComponent.AddHandler(HrEventType.EVENT_LOAD_SCENE_RESOURCE_SUCCESS, HandleLoadSceneAssetBundleSuccess);
                HrGameWorld.Instance.SceneComponent.LoadSceneSync(m_nPreloadSceneResID);
            }

            yield return null;

            //等待场景AssetBundle加载完毕
            while (!m_bNextScenePrepared)
            {
                yield return null;
            }

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
            m_lisLoadedResource.Add(res.AssetName);

            int nTotalResCount = m_lisPreloadResourceID.Count;
            int nLoadedResCount = m_lisLoadedResource.Count;

            HrGameWorld.Instance.EventComponent.SendEvent(this, new HrEventPreloadProgressEventHandler((int)EnumEventType.EVENT_PRELOADING_PROGRESS, null, nTotalResCount, nLoadedResCount));
        }

        private void LoadResourceFailed(string strResourceName, string strErrorMsg)
        {
            HrLogger.LogError(string.Format("load res '{0}' error! error msg '{1}'", strResourceName, strErrorMsg));
        }

        private void OnPreloadFinished()
        {
            //HrGameWorld.Instance.SceneComponent.SwitchToScene<Hr.Scene.HrSceneWorld>();
            HrLogger.Log("Load Res Success!!!!!!!!!!!!!!");
        }

        private void HandleLoadSceneAssetBundleSuccess(object sender, HrEventHandlerArgs args)
        {
            HrGameWorld.Instance.EventComponent.RemoveHandler(HrEventType.EVENT_LOAD_SCENE_RESOURCE_SUCCESS, HandleLoadSceneAssetBundleSuccess);
            m_bNextScenePrepared = true;
        }

    }
}
