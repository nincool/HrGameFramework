using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.EventSystem
{
    public delegate void HrEventListener(int e, params object[] args);

    public interface IEventManager
    {
        void AddHandler(int nEvent, EventHandler<HrEventHandlerArgs> handler);

        void SendEvent(object sender, HrEventHandlerArgs args);

        void RemoveHandler(int nEvent, EventHandler<HrEventHandlerArgs> handler);

    }
}