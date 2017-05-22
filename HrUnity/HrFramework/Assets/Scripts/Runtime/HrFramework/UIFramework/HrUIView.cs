using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public class HrUIView : MonoBehaviour
    {
        protected EnumView m_viewType = EnumView.VIEW_UNKNOW;

        public EnumView ViewType
        {
            get
            {
                return m_viewType;
            }
        }

        /// <summary>
        /// 首次挂载界面
        /// </summary>
        public virtual void OnEnter()
        {
        }

        /// <summary>
        /// 界面显示
        /// </summary>
        public virtual void Show()
        {
            HrLogger.Log("HrUIView Show!!!!!");
            this.gameObject.SetActive(true);
        }

        /// <summary>
        ///Update 
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// 界面隐藏
        /// </summary>
        public virtual void Hide()
        {
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// 界面销毁退出
        /// </summary>
        public virtual void OnExit()
        {
        }
    }

}
