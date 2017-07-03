using Hr.Define;
using Hr.EventSystem;
using Hr.Resource;
using System;
using System.Collections.Generic;

namespace Hr.UI
{
    public class HrUIManager : HrModule, IUIManager
    {
        private Dictionary<int, IUILogic> m_dicUILogic = new Dictionary<int, IUILogic>();

        /// <summary>
        /// 当前显示的UI界面栈
        /// </summary>
        private Stack<IUILogic> m_staCurrentUILogic = new Stack<IUILogic>();

        /// <summary>
        /// 场景中UI节点
        /// </summary>
        private HrUIRoot m_uiRoot = new HrUIRoot();

        public override void Init()
        {

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
            if (uiLogic != null)
            {
                uiLogic.AttachUIView(uiView);
            }
            else
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
                uiLogic = (IUILogic)Activator.CreateInstance(tLogic);
                uiLogic.AttachUIView(uiView);
                uiLogic.OnEnter();

                m_dicUILogic.Add(uiView.UIID, uiLogic);

            }
        }

        public void AttachUIRoot()
        {
            m_uiRoot.AttachUIAnchor();
        }

        #region private methods

        private void RegistEventListener()
        {
            HrGameWorld.Instance.EventComponent.AddHandler(HrEventType.EVENT_UI_SHOW, HandleShowUI);
            //HrEventManager.Instance.AddListener(HandleCreateUI, EnumEvent.EVENT_UI_CREATE);

            //HrEventManager.Instance.AddListener(OnUICreate, EnumEvent.EVENT_UI_ONCREATE);
            //HrEventManager.Instance.AddListener(OnUIDestroy, EnumEvent.EVENT_UI_ONDESTROY);

            //HrEventManager.Instance.AddListener(HandleUIShow, EnumEvent.EVENT_UI_SHOW);
            //HrEventManager.Instance.AddListener(HandleUIHide, EnumEvent.EVENT_UI_HIDE);
        }

        private void HandleShowUI(object sender, HrEventHandlerArgs args)
        {
            int nPanelType = (int)args.UserData;

            var uiLogic = m_dicUILogic.HrTryGet(nPanelType);
            if (uiLogic == null)
            {
                HrLogger.Log(string.Format("ready show ui {0}, but the res is not loaded! so we will load the res first", uiLogic));

                CreateUI(nPanelType);
            }
        
        }

        private void CreateUI(int nPanelType)
        {
            //HrResource uiResource = HrGameWorld.Instance.ResourceComponent.GetResource(nPanelType);
            //HrGameObjectUtil.Instantiate(uiResource) 
        }
        #endregion
    }

}
