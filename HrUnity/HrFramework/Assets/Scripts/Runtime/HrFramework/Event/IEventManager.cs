using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public delegate void HrEventListener(int e, params object[] args);

    public interface IEventManager
    {
        /// <summary>
        /// 添加监听者
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="events"></param>
        void AddListener(HrEventListener handler, params int[] events);

        /// <summary>
        /// 删除监听者
        /// </summary>
        /// <param name="e"></param>
        /// <param name="listener"></param>
        void RemoveListener(int e, HrEventListener listener);

        /// <summary>
        /// 清除所有监听者
        /// </summary>
        void ClearHandler();

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="e"></param>
        /// <param name="args"></param>
        void SendEvent(int e, params object[] args);

    }
}