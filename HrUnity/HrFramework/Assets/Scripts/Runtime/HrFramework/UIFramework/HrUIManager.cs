using Hr;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public class HrUIManager : HrSingleton<HrUIManager>
    {
        /// <summary>
        /// 存储所有面板资源路径， 从配置文件中读取 TODO 配置文件读取
        /// </summary>
        private Dictionary<EnumView, string> m_dicViewAssetPath = new Dictionary<EnumView, string>();

        /// <summary>
        /// 存储所有加载过的面板
        /// </summary>
        private Dictionary<EnumView, HrUIMediator> m_dicUIMediator = new Dictionary<EnumView, HrUIMediator>();

        public void Init()
        {
            ///For testing
            m_dicViewAssetPath.Add(EnumView.VIEW_LAUNCH_LOADING, "assets/media/ui/uiprefab/panelloading.prefab");

            InitUI();

            //注册侦听消息 
            RegistEventListener();

        }
        private void InitUI()
        {
            m_dicUIMediator.Add(EnumView.VIEW_LAUNCH_LOADING, new HrUILoadingMediator());

        }

        private void RegistEventListener()
        {
            //HrEventManager.Instance.AddListener(HandleCreateUI, EnumEvent.EVENT_UI_CREATE);

            //HrEventManager.Instance.AddListener(OnUICreate, EnumEvent.EVENT_UI_ONCREATE);
            //HrEventManager.Instance.AddListener(OnUIDestroy, EnumEvent.EVENT_UI_ONDESTROY);

            //HrEventManager.Instance.AddListener(HandleUIShow, EnumEvent.EVENT_UI_SHOW);
            //HrEventManager.Instance.AddListener(HandleUIHide, EnumEvent.EVENT_UI_HIDE);
        }

        /// <summary>
        /// 逻辑更新
        /// </summary>
        public void LogicUpdate()
        {
            var enuMediator = m_dicUIMediator.GetEnumerator();
            while (enuMediator.MoveNext())
            {
                enuMediator.Current.Value.Update();
            }
        }

        #region EventHandler
        private void HandleCreateUI(int e, params object[] args)
        {
            EnumView viewType = (EnumView)args[0];
            var uiMediator = m_dicUIMediator.HrTryGet(viewType);
            if (uiMediator == null)
            {
                HrLogger.LogError("HrUIManager HandleCreateUI! ViewType:" + viewType);
                return;
            }
            if (uiMediator.View == null)
            {
                string strUIAssetPath = m_dicViewAssetPath.HrTryGet(viewType);
                if (strUIAssetPath != null)
                {
                    GameObject obPanel = HrResourceManager.Instance.LoadAsset<GameObject>(strUIAssetPath);
                    if (obPanel != null)
                    {
                        HrGameObjectUtil.InstantiateUI(obPanel, GetUIAnchor(EnumUIAnchor.ANCHOR_CENTER));
                    }
                    else
                        HrLogger.LogError("HrUIManager HandleCreateUI Error! Asset is null:" + strUIAssetPath);
                }
                else
                {
                    HrLogger.LogError("HrUIManager HandleCureateUI Error! ViewType:" + viewType);
                }
            }
            else
            {
                HrLogger.LogError("HrUIManager HandleCreateUI View Alread Created! ViewType:" + viewType);
            }
        }

        private void OnUICreate(int e, params object[] args)
        {
            HrUIView uiView = args[0] as HrUIView;
            if (uiView == null)
            {
                HrLogger.LogError("HrUIManager OnUICreate Error!");
                return;
            }
            HrUIMediator mediator = m_dicUIMediator.HrTryGet(uiView.ViewType);
            mediator.View = uiView;
        }

        private void OnUIDestroy(int e, params object[] args)
        {

        }
        
        private void HandleUIShow(int e, params object[] args)
        {
            EnumView viewType = (EnumView)args[0];
            var uiMediator = m_dicUIMediator.HrTryGet(viewType);
            if (uiMediator == null)
            {
                HrLogger.LogError("HrUIManager Show Error! ViewType:" + viewType);
                return;
            }

            uiMediator.Show();
        }

        private void HandleUIHide(int e, params object[] args)
        {
            EnumView viewType = (EnumView)args[0];
            var uiMediator = m_dicUIMediator.HrTryGet(viewType);
            if (uiMediator == null)
            {
                HrLogger.LogError("HrUIManager Show Hide! ViewType:" + viewType);
                return;
            }

            uiMediator.Hide();
        }
        #endregion

        private Transform GetUIAnchor(EnumUIAnchor anchor)
        {
            return null;
            //return HrGameWorld.Instance.SceneManager.CurrentScene.UIRoot.GetAnchor(anchor);
        }
    }

}
