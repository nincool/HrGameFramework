using Hr.EventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public sealed class HrEventComponent : HrComponent
    {
        public bool InitSuccess { get; private set; }

        private IEventManager m_eventManager = null;

        protected override void Awake()
        {
            base.Awake();

            m_eventManager = HrGameWorld.Instance.GetModule<HrEventManager>();
            if (m_eventManager != null)
            {
                InitSuccess = true;
            }
        }

        public void AddHandler(int nEvent, EventHandler<HrEventHandlerArgs> handler)
        {
            m_eventManager.AddHandler(nEvent, handler);
        }

        public void SendEvent(object sender, HrEventHandlerArgs args)
        {
            m_eventManager.SendEvent(sender, args);
        }


    }
}
