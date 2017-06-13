using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

namespace Hr
{
    public sealed class HrEventManager : HrModule, IEventManager
    {

        /// <summary>
        /// 存储所有监听者，事件ID对应监听者们
        /// </summary>
        private Dictionary<int, HashSet<HrEventListener>> m_dicListener = new Dictionary<int, HashSet<HrEventListener>>();

        public HrEventManager()
        {
        }

        public override void Init()
        {

        }

        public void AddListener(HrEventListener handler, params int[] events)
        {
            foreach (var e in events)
            {
                var hasListener = m_dicListener.HrTryGet(e);
                if (hasListener == null)
                {
                    hasListener = new HashSet<HrEventListener>();
                    m_dicListener.Add(e, hasListener);
                }
                hasListener.Add(handler);
            }
        }

        public void RemoveListener(int e, HrEventListener listener)
        {
            var hasListener = m_dicListener.HrTryGet(e);
            if (hasListener != null)
            {
                hasListener.Remove(listener);
            }
        }

        public void ClearHandler()
        {
            m_dicListener.Clear();
        }

        public void SendEvent(int e, params object[] args)
        {
            var hasListener = m_dicListener.HrTryGet(e);
            if (hasListener != null)
            {
                //有可能在update中调用，所以尽量避免foreach
                var enuListener = hasListener.GetEnumerator();
                while (enuListener.MoveNext())
                {
                    enuListener.Current(e, args);
                }
            }
        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {

        }
    }

}
