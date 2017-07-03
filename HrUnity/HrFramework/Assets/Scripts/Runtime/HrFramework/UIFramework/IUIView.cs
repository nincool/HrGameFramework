using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.UI
{
    public interface IUIView
    {
        /// <summary>
        /// 首次挂载界面
        /// </summary>
        void OnEnter();


        /// <summary>
        /// 界面显示
        /// </summary>
        void Show();


        /// <summary>
        ///Update 
        /// </summary>
        void OnUpdate(float fElapseSeconds, float fRealElapseSeconds);


        /// <summary>
        /// 界面隐藏
        /// </summary>
        void Hide();


        /// <summary>
        /// 界面销毁退出
        /// </summary>
        void OnExit();

    }
}
