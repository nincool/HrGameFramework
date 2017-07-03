using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hr.UI;

namespace Hr
{
    public class HrUIComponent : HrComponent
    {
        public bool InitSuccess { get; private set; }

        private IUIManager m_uiManager;

        public void AttachUIRoot()
        {
            m_uiManager.AttachUIRoot();
        }

        public void RegisterUIView(HrUIView uiView)
        {
            m_uiManager.RegisterUIView(uiView);
        }

        protected override void Awake()
        {
            base.Awake();

            m_uiManager = HrGameWorld.Instance.GetModule<HrUIManager>();
            if (m_uiManager == null)
            {
                InitSuccess = false;
            }
        }

        protected override void Start()
        {
            base.Start();
        }

        

    }
}
