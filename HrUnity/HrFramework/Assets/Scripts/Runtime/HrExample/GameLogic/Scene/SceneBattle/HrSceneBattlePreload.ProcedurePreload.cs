using Hr.DataTable;
using Hr.Define;
using Hr.EventSystem;
using Hr.Resource;
using Hr.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Scene.Procedure.HrSceneBattlePreload
{
    public class HrProcedurePreload : HrProcedure
    {
        private int m_nPreloadSceneResID = -1;
        private List<int> m_lisPreloadResourceID = new List<int>();
        private List<string> m_lisLoadedResource = new List<string>();

        private bool m_bNextScenePrepared = false;

        private HrLoadResourceCallBack m_loadResourceCallBack;

        public HrProcedurePreload(HrScene owner) : base(owner)
        {
            m_loadResourceCallBack = new HrLoadResourceCallBack(LoadResourceSuccess, LoadResourceFailed);
        }

        public override void OnEnter()
        {
            HrGameWorld.Instance.EventComponent.SendEvent(this, new HrEventUIViewEventHandler(HrEventType.EVENT_UI_SHOW, null, (int)EnumUIType.UITYPE_LOADING_VIEW));

            PreloadResources();
        }

        private void PreloadResources()
        {
            var dataTable = HrGameWorld.Instance.DataTableComponent.GetDataTable(HrDataTablePreloadBattleScene.DataTableDefaultName) as HrDataTablePreloadBattleScene;
            if (dataTable != null)
            {
                foreach (var itemPreloadResouceInfo in dataTable.PreloadBattleSceneInfo)
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
                HrGameWorld.Instance.SceneComponent.LoadUnitySceneSync(m_nPreloadSceneResID);
            }

            yield return null;

            //等待场景AssetBundle加载完毕
            while (!m_bNextScenePrepared)
            {
                yield return null;
            }

            OnPreloadFinished();
        }

        private void OnPreloadFinished()
        {
            HrGameWorld.Instance.UIComponent.Reset();

            HrGameWorld.Instance.SceneComponent.UnloadUnityCurrentScene();
            HrGameWorld.Instance.SceneComponent.SwitchToScene<Hr.Scene.HrSceneBattle>();
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

        private void HandleLoadSceneAssetBundleSuccess(object sender, HrEventHandlerArgs args)
        {
            HrGameWorld.Instance.EventComponent.RemoveHandler(HrEventType.EVENT_LOAD_SCENE_RESOURCE_SUCCESS, HandleLoadSceneAssetBundleSuccess);
            m_bNextScenePrepared = true;
        }
    }
}




