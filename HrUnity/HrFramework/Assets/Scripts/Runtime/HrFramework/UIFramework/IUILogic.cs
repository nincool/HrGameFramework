using Hr.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.UI
{
    public interface IUILogic
    {
        int ViewID
        {
            get;
        }

        EnumUIViewType ViewType
        {
            get;
            set;
        }

        EnumUIShowMode ShowMode
        {
            get;
            set;
        }

        EnumUIColliderMode ColliderMode
        {
            get;
            set;
        }

        void AttachUIView(HrUIView uiView);

        /// <summary>
        /// 首次挂载界面
        /// </summary>
        void OnEnter();

        /// <summary>
        /// 界面显示
        /// </summary>
        void Show();

        /// <summary>
        /// 再次显示
        /// </summary>
        void Reshow();

        /// <summary>
        ///Update 
        /// </summary>
        void OnUpdate(float fElapseSeconds, float fRealElapseSeconds);

        /// <summary>
        /// 冻结状态
        /// </summary>
        void Freeze();

        /// <summary>
        /// 解冻状态
        /// </summary>
        void Unfreeze();

        /// <summary>
        /// 界面隐藏
        /// </summary>
        void Hide();

        /// <summary>
        /// 界面移除
        /// </summary>
        void OnExit();
    }
}
