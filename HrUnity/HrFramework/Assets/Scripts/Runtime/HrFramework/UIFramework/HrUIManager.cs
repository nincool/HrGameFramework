using Hr.Define;
using Hr.EventSystem;
using Hr.Resource;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.UI
{
    public class HrUIManager : HrModule, IUIManager
    {
        private Dictionary<int, IUILogic> m_dicUILogic = new Dictionary<int, IUILogic>();

        /// <summary>
        /// 当前显示的 UI界面
        /// </summary>
        private Dictionary<int, IUILogic> m_dicCurrentShowUILogic = new Dictionary<int, IUILogic>();

        /// <summary>
        /// 当前显示的Popup UI界面栈
        /// </summary>
        private Stack<int> m_staUILogicID = new Stack<int>();

        /// <summary>
        /// 场景中UI节点
        /// </summary>
        private HrUIRoot m_uiRoot = new HrUIRoot();

        /// <summary>
        /// 加载UI资源回调
        /// </summary>
        private HrLoadResourceCallBack m_loadUIResourceCallBack;

        public HrUIManager()
        {
            m_loadUIResourceCallBack = new HrLoadResourceCallBack(LoadResourceSuccess, LoadResourceFailed);
        }

        public override void Init()
        {
            RegistEventListener();
        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
            
        }

        public override void Shutdown()
        {

        }

        public void RegisterUIView(HrUIView uiView)
        {
            if (uiView.UIID == 0)
            {
                HrLogger.LogError(string.Format("when an ui[{0}] register to uimanager, the uimanager find the uiview's id is zeor!", uiView.GetType().FullName));
                return;
            }

            IUILogic uiLogic = m_dicUILogic.HrTryGet(uiView.UIID);
            if (uiLogic == null)
            {
                CreateUILogic(uiView);
            }
            else
            {
                uiLogic.AttachUIView(uiView);
                uiLogic.OnEnter();
            }
        }

        public void AttachUIRoot()
        {
            m_uiRoot.AttachUIAnchor();
        }

        public void Clear()
        {
            //隐藏所有UI
            HideAllUIView();
            //清空当前显示UI容器
            ClearCurrentShowUI();
            //情况栈信息
            ClearStackArray();

            foreach (var itemLogic in m_dicUILogic)
            {
                itemLogic.Value.OnExit();
            }
            m_dicUILogic.Clear();
        }

        #region private methods

        private void RegistEventListener()
        {
            HrGameWorld.Instance.EventComponent.AddHandler(HrEventType.EVENT_UI_SHOW, HandleShowUI);
            HrGameWorld.Instance.EventComponent.AddHandler(HrEventType.EVENT_UI_HIDE, HandleHideUI);
        }

        private void HandleShowUI(object sender, HrEventHandlerArgs args)
        {
            HrEventUIViewEventHandler showUIViewArgs = args as HrEventUIViewEventHandler;

            int nPanelType = showUIViewArgs.PanelType;

            var uiLogic = m_dicUILogic.HrTryGet(nPanelType);
            if (uiLogic == null)
            {
                HrLogger.Log(string.Format("ready show ui {0}, but the res is not loaded! so we will load the res first", nPanelType));

                CreateUI(nPanelType);
                uiLogic = m_dicUILogic.HrTryGet(nPanelType);
            }
   
            if (uiLogic != null)
            {
                ShowUIView(uiLogic);
            }
            else
            {
                throw new HrException(string.Format("uimanager handle show ui error! can not create the uilogic for panenal[{0}]", nPanelType));
            }
            
        }

        private void HandleHideUI(object sender, HrEventHandlerArgs args)
        {
            HrEventUIViewEventHandler showUIViewArgs = args as HrEventUIViewEventHandler;

            int nPanelType = showUIViewArgs.PanelType;

            var uiLogic = m_dicUILogic.HrTryGet(nPanelType);
            if (uiLogic == null)
            {
                HrLogger.Log(string.Format("ready hide ui {0}, but can't find the logic!", nPanelType));
                return;
            }
            HideUIView(uiLogic);
        }

        private void CreateUI(int nPanelType)
        {
            HrResourcePrefab uiResource = HrGameWorld.Instance.ResourceComponent.GetResource(nPanelType) as HrResourcePrefab;
            if (uiResource == null)
            {
                HrGameWorld.Instance.ResourceComponent.LoadResourceSync(nPanelType, m_loadUIResourceCallBack);
            }
            else
            {
                CreateUIGameObject(uiResource);
            }
        }

        private IUILogic CreateUILogic(HrUIView uiView)
        {
            string strViewFullName = uiView.GetType().FullName;
            int nSubStringIndex = strViewFullName.LastIndexOf("View");
            if (nSubStringIndex == -1)
            {
                throw new HrException(string.Format("find an error ui view! it's name '{0}' is invalid! ", strViewFullName));
            }
            string strLogicFullName = strViewFullName.Substring(0, nSubStringIndex);
            Type tLogic = Type.GetType(strLogicFullName + "Logic");
            if (tLogic == null)
            {
                throw new HrException(string.Format("find an error ui logic! it's name '{0}' is invalid!", strLogicFullName));
            }
            IUILogic uiLogic = (IUILogic)Activator.CreateInstance(tLogic);
            uiLogic.AttachUIView(uiView);
            uiLogic.OnEnter();

            if (m_dicUILogic.ContainsKey(uiView.UIID))
            {
                throw new HrException(string.Format("create ui logic error! already exists {0} ID ", uiView.UIID));
            }
                
            m_dicUILogic.Add(uiView.UIID, uiLogic);

            return uiLogic;
        }

        private void LoadResourceSuccess(HrResource res)
        {
            HrLogger.Log(string.Format("load res '{0}' success", res.AssetName));
            CreateUIGameObject(res as HrResourcePrefab);
        }

        private void LoadResourceFailed(string strResourceName, string strErrorMsg)
        {
            HrLogger.LogError(string.Format("load res '{0}' error! error msg '{1}'", strResourceName, strErrorMsg));
        }

        private void CreateUIGameObject(HrResourcePrefab uiResource)
        {
            var uiViewObject = HrGameObjectUtil.InstantiateUI(uiResource.InstancePrefab, m_uiRoot.GetAnchor(EnumUIRoot.UI_ROOT_NORMALROOT));

            HrUIView uiView = uiViewObject.GetComponent<HrUIView>();
            if (null == uiView)
            {
                HrLogger.LogError(string.Format("the view panel '{0}' does not contain logic component", uiResource.AssetName));
            }
            var uiLogic = m_dicUILogic.HrTryGet(uiView.UIID);
            if (uiLogic == null)
            {
                CreateUILogic(uiView);
            }
        }

        private void ShowUIView(IUILogic uiLogic)
        {
            switch (uiLogic.ShowMode)
            {
                case EnumUIShowMode.UI_SHOWMODE_DONOTHING:
                    {
                        ShowAndAddUILogic(uiLogic);

                        break;
                    }
                case EnumUIShowMode.UI_SHOWMODE_HIDEOTHER:
                    {
                        HideAllUIView();
                        ShowAndAddUILogic(uiLogic);

                        break;
                    }
                case EnumUIShowMode.UI_SHOWMODE_NEEDBACK:
                    {
                        PushUIToStack(uiLogic);
                        ShowAndAddUILogic(uiLogic);

                        break;
                    }
            }

        }

        private void HideUIView(IUILogic uiLogic)
        {
            switch (uiLogic.ShowMode)
            {
                case EnumUIShowMode.UI_SHOWMODE_DONOTHING:
                    {
                        HideAndRemoveUILogic(uiLogic);

                        break;
                    }
                case EnumUIShowMode.UI_SHOWMODE_HIDEOTHER:
                    {
                        ShowAllUIView();
                        HideAndRemoveUILogic(uiLogic);

                        break;
                    }
                case EnumUIShowMode.UI_SHOWMODE_NEEDBACK:
                    {
                        PopUIFromStack();

                        break;
                    }
            }
        }

        private void HideAllUIView()
        {
            foreach (var iteLogic in m_dicCurrentShowUILogic)
            {
                iteLogic.Value.Hide();
            }
        }

        private void ShowAllUIView()
        {
            foreach (var iteLogic in m_dicCurrentShowUILogic)
            {
                iteLogic.Value.Reshow();
            }
        }

        private void PushUIToStack(IUILogic uiLogic)
        {
            if (m_staUILogicID.Count > 0)
            {
                int nID = m_staUILogicID.Peek();
                var uiTopLogic = m_dicCurrentShowUILogic.HrTryGet(nID);
                uiTopLogic.Freeze();
            }

            m_staUILogicID.Push(uiLogic.ViewID);
        }

        private void PopUIFromStack()
        {
            if (m_staUILogicID.Count > 2)
            {
                int nID = m_staUILogicID.Pop();
                IUILogic topUILogic = m_dicCurrentShowUILogic.HrTryGet(nID);
                if (topUILogic != null)
                {
                    HideAndRemoveUILogic(topUILogic);
                }
                int nNextID = m_staUILogicID.Peek();
                IUILogic nextUILogic = m_dicCurrentShowUILogic.HrTryGet(nID);
                nextUILogic.Unfreeze();
            }
            else if (m_staUILogicID.Count == 1)
            {
                int nID = m_staUILogicID.Pop();
                IUILogic topUILogic = m_dicCurrentShowUILogic.HrTryGet(nID);
                if (topUILogic != null)
                {
                    HideAndRemoveUILogic(topUILogic);
                }
            }
        }

        private void ShowAndAddUILogic(IUILogic uiLogic)
        {
            uiLogic.Show();
            m_dicCurrentShowUILogic.Add(uiLogic.ViewID, uiLogic);
        }

        private void HideAndRemoveUILogic(IUILogic uiLogic)
        {
            uiLogic.Hide();
            if (m_dicCurrentShowUILogic.ContainsKey(uiLogic.ViewID))
            {
                m_dicCurrentShowUILogic.Remove(uiLogic.ViewID);
            }
        }

        private void ClearStackArray()
        {
            m_staUILogicID.Clear();
        }

        private void ClearCurrentShowUI()
        {
            m_dicCurrentShowUILogic.Clear();
        }

        #endregion
    }
}

