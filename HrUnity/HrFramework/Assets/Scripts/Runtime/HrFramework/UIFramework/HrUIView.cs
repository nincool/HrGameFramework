using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.UI
{
    /// <summary>
    /// UI 显示模式
    /// </summary>
    public enum EnumUIMode
    {
        UIMODE_DYNAMIC_POPUP = 0x1,        //动态弹出界面 弹出压入堆栈
    }

    public class HrUIView : MonoBehaviour, IUIView
    {
        /// <summary>
        /// UI 唯一ID
        /// </summary>
        public virtual int UIID
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// UI 模式
        /// </summary>
        public uint UIMode
        {
            get;
            set;
        } 

        public virtual void Awake()
        {
            HrGameWorld.Instance.UIComponent.RegisterUIView(this);
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
            if (!this.gameObject.activeSelf)
                this.gameObject.SetActive(true);
        }

        /// <summary>
        ///Update 
        /// </summary>
        public virtual void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
        }

        /// <summary>
        /// 界面隐藏
        /// </summary>
        public virtual void Hide()
        {
            if (this.gameObject.activeSelf)
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
