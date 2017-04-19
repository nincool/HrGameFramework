using System.Collections.Generic;
using System.Collections;
using Hr;
using System;
using UnityEngine;

public class HrEventManager : HrSingleton<HrEventManager>
{
    public delegate void HrEventListener(EnumEvent e, params object[] args);

    /// <summary>
    /// 
    /// </summary>
    private Dictionary<EnumEvent, HashSet<HrEventListener>> m_dicListener = new Dictionary<EnumEvent, HashSet<HrEventListener>>();

    public HrEventManager()
    {

    }

    public void AddListener(HrEventListener handler, params EnumEvent[] events)
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

    public void RemoveListener(EnumEvent e, HrEventListener listener)
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

    public void SendEvent(EnumEvent e, params object[] args)
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
}
