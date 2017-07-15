using Hr.Define;
using System.Collections;
using System.Collections.Generic;

namespace Hr.UI
{
    public class HrUILogic : IUILogic
    {
        protected HrUIView m_view;

        public int ViewID
        {
            get
            {
                return m_view == null ? 0 : m_view.UIID;
            }
        }

        public EnumUIViewType ViewType
        {
            get;
            set;
        }

        public EnumUIShowMode ShowMode
        {
            get;
            set;
        }

        public EnumUIColliderMode ColliderMode
        {
            get;
            set;
        }


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
        
        public HrUILogic()
        {
            m_view = null;
            ViewType = EnumUIViewType.UI_VIEWTYPE_NORMAL;
            ShowMode = EnumUIShowMode.UI_SHOWMODE_DONOTHING;
            ColliderMode = EnumUIColliderMode.UI_COLLIDER_NONE;
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

        public virtual void Reshow()
        {
            if (m_view != null)
                m_view.Reshow();
        }

        /// <summary>
        /// Update 
        /// </summary>
        public virtual void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
            if (m_view != null)
                m_view.OnUpdate(fElapseSeconds, fRealElapseSeconds);
        }

        public virtual void Freeze()
        {
            if (m_view != null)
                m_view.Freeze();
        }

        public virtual void Unfreeze()
        {

        }

        /// <summary>
        /// 界面隐藏
        /// </summary>
        public virtual void Hide()
        {
            if (m_view != null)
                m_view.Hide();
        }

        public virtual void OnExit()
        {
            if (m_view != null)
            {
                m_view.OnExit();
            }
        }

    }
}
