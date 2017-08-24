using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using Hr.EventSystem;

namespace Hr.EventSystem
{
    public sealed class HrEventManager : HrModule, IEventManager
    {

        private readonly Dictionary<int, EventHandler<HrEventHandlerArgs>> m_dicHandlers = new Dictionary<int, EventHandler<HrEventHandlerArgs>>();

        private readonly Queue<HrEvent> m_queEvents = new Queue<HrEvent>();

        public HrEventManager()
        {
        }

        public override void Init()
        {

        }

        public void AddHandler(int nEvent, EventHandler<HrEventHandlerArgs> handler)
        {
            if (handler == null)
            {
                HrLogger.LogError("Add Handler Error! handler is null!");
                return;
            }

            if (CheckIsInHandlerList(nEvent, handler))
            {
                HrLogger.LogError("Add Handler Error! handler is in handlerlist!");
                return;
            }

            var eventHandler = m_dicHandlers.HrTryGet(nEvent);
            if (eventHandler != null)
            {
                eventHandler += handler;
                //是否为引用
                m_dicHandlers[nEvent] = eventHandler;
            }
            else
            {
                m_dicHandlers[nEvent] = handler;
            }
        }

        public void RemoveHandler(int nEvent, EventHandler<HrEventHandlerArgs> handler)
        {
            if (handler == null)
            {
                HrLogger.LogError("Remove Handler Error! handler is null!");
                return;
            }

            if (m_dicHandlers.ContainsKey(nEvent))
            {
                m_dicHandlers[nEvent] -= handler;
            }
        }

        public void SendEventAsync(object sender, HrEventHandlerArgs args)
        {
            HrEvent hrEvent = new HrEvent(sender, args);
            lock (m_queEvents)
            {
                m_queEvents.Enqueue(hrEvent);
            }
        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
            while (m_queEvents.Count > 0)
            {
                HrEvent e = null;
                lock (m_queEvents)
                {
                    e = m_queEvents.Dequeue();
                }

                HandleEvent(e.Sender, e.EventArgs);
            }
        }

        public override void OnUpdateEndOfFrame(float fElapseSeconds, float fRealElapseSeconds)
        {
        }

        public bool CheckIsInHandlerList(int nEventID, EventHandler<HrEventHandlerArgs> handler)
        {
            if (handler == null)
            {
                HrLogger.LogError("handler is null");
                return false;
            }

            var eventHandler = m_dicHandlers.HrTryGet(nEventID);
            if (null != eventHandler)
            {
                var lisHandlerList = eventHandler.GetInvocationList();
                foreach (EventHandler<HrEventHandlerArgs> itemHandler in lisHandlerList)
                {
                    if (itemHandler == handler)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override void Shutdown()
        {
            lock (m_queEvents)
            {
                m_queEvents.Clear();
            }
            m_dicHandlers.Clear();
        }

        public void SendEvent(object sender, HrEventHandlerArgs args)
        {
            HandleEvent(sender, args);
        }

        private void HandleEvent(object sender, HrEventHandlerArgs args)
        {
            var handler = m_dicHandlers.HrTryGet(args.EventID);
            if (handler != null)
            {
                handler(sender, args);
                return;
            }
        }
    }

}
