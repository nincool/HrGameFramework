using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public class HrUIComponent : HrComponent
    {

        public bool InitSuccess { get; private set; }

        private HrUIManager m_uiManager;

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
