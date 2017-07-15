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
        /// 再次显示
        /// </summary>
        void Reshow();

        /// <summary>
        ///Update 
        /// </summary>
        void OnUpdate(float fElapseSeconds, float fRealElapseSeconds);

        /// <summary>
        /// 界面冻结
        /// </summary>
        void Freeze();

        /// <summary>
        /// 界面解冻
        /// </summary>
        void Unfreeze();

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
