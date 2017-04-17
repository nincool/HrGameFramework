using System.Collections.Generic;
using System.Collections;
using Hr.CommonUtility;
using System;
using UnityEngine;

public class HrEventManager : HrSingleton<HrEventManager>
{
    public delegate void HrEventListener(string strEvent, params object[] args);

    /// <summary>
    /// 
    /// </summary>
    private Dictionary<string, HashSet<HrEventListener>> m_dicListener = new Dictionary<string, HashSet<HrEventListener>>();

    public HrEventManager()
    {

    }

    public void AddListener(HrEventListener handler, params string[] strEvents)
    {
        foreach (var strEvent in strEvents)
        {
            var hasListener = m_dicListener.HrTryGet(strEvent);
            if (hasListener == null)
            {
                hasListener = new HashSet<HrEventListener>();
                m_dicListener.Add(strEvent, hasListener);
            }
            hasListener.Add(handler);
        }
    }

    public void RemoveListener(string strEvent, HrEventListener listener)
    {
        var hasListener = m_dicListener.HrTryGet(strEvent);
        if (hasListener != null)
        {
            hasListener.Remove(listener);
        }
    }

    public void ClearHandler()
    {
        m_dicListener.Clear();
    }

    public void SendEvent(string strEvent, params object[] args)
    {
        var hasListener = m_dicListener.HrTryGet(strEvent);
        if (hasListener != null)
        {
            //有可能在update中调用，所以尽量避免foreach
            var enuListener = hasListener.GetEnumerator();
            while (enuListener.MoveNext())
            {
                enuListener.Current(strEvent, args);
            }
        }
    }
}
