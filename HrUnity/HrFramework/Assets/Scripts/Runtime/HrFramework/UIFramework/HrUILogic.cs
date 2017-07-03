using System.Collections;
using System.Collections.Generic;


namespace Hr.UI
{
    public class HrUILogic : IUILogic
    {
        protected HrUIView m_view;



        public HrUIView View
        {
            get
            {
                if (m_view != null)
                {
                    return m_view;
                }
                HrLogger.LogError("HrUIMediator Error!");

                return null;
            }
            protected set
            {
                m_view = value;
            }
        }


        public virtual void AttachUIView(HrUIView uiView)
        {
            View = uiView;
        }

        public virtual void OnEnter()
        {
            if (m_view != null)
            {
                m_view.OnEnter();
            }
        }

        /// <summary>
        /// 界面显示
        /// </summary>
        public virtual void Show()
        {
            if (m_view != null)
                m_view.Show();
        }

        /// <summary>
        ///Update 
        /// </summary>
        public virtual void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
            if (m_view != null)
                m_view.OnUpdate(fElapseSeconds, fRealElapseSeconds);
        }

        /// <summary>
        /// 界面隐藏
        /// </summary>
        public virtual void Hide()
        {
            if (m_view != null)
                m_view.Hide();
        }

    }
}
