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
            InitUI();

            //注册侦听消息 
            RegistEventListener();

        }

        private void RegistEventListener()
        {
            HrEventManager.Instance.AddListener(HandleCreateUI, EnumEvent.EVENT_UI_CREATE);

            HrEventManager.Instance.AddListener(OnUICreate, EnumEvent.EVENT_UI_ONCREATE);
            HrEventManager.Instance.AddListener(OnUIDestroy, EnumEvent.EVENT_UI_ONDESTROY);

            HrEventManager.Instance.AddListener(HandleUIShow, EnumEvent.EVENT_UI_SHOW);
            HrEventManager.Instance.AddListener(HandleUIHide, EnumEvent.EVENT_UI_HIDE);


        }

        private void InitUI()
        {
            m_dicUIMediator.Add(EnumView.VIEW_LAUNCH_LOADING, new HrUILoadingMediator());

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
        private void HandleCreateUI(EnumEvent e, params object[] args)
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
                    //HrResourceManager.Instance.LoadAssetBundleSync(strUIAssetPath);
                }
            }
            else
            {
                HrLogger.LogError("HrUIManager HandleCreateUI ViewType:" + viewType);
            }
        }

        private void OnUICreate(EnumEvent e, params object[] args)
        {

        }

        private void OnUIDestroy(EnumEvent e, params object[] args)
        {

        }
        
        private void HandleUIShow(EnumEvent e, params object[] args)
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

        private void HandleUIHide(EnumEvent e, params object[] args)
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
    }

}
