using System.Collections;
using System.Collections.Generic;


namespace Hr
{
    public class HrUIMediator
    {
        protected HrUIView m_view;

        public HrUIView View
        {
            get
            {
                if (m_view != null && m_view is HrUIView)
                {
                    return m_view;
                }
                HrLogger.LogError("HrUIMediator Error!");
                return null;
            }
        }

        /// <summary>
        /// 首次挂载界面
        /// </summary>
        public virtual void OnEnter()
        {
            if (m_view != null)
                m_view.OnEnter();
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
        public virtual void Update()
        {
            if (m_view != null)
                m_view.Update();
        }

        /// <summary>
        /// 界面隐藏
        /// </summary>
        public virtual void Hide()
        {
            if (m_view != null)
                m_view.Hide();
        }

        /// <summary>
        /// 界面销毁退出
        /// </summary>
        public virtual void OnExit()
        {
            if (m_view != null)
                m_view.OnExit();
        }
    }
}
