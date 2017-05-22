using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public class HrFSMComponent : HrComponent
    {
        public bool InitSuccess { get; private set; }

        private HrFSMManager m_fsmManager = null;

        protected override void Awake()
        {
            base.Awake();

            m_fsmManager = HrGameWorld.Instance.GetModule<HrFSMManager>();
            if (m_fsmManager != null)
                InitSuccess = true;
        }

        public IFSMStateMachine CreateFSM<T>(string strName, T owner) where T : class
        {
            return m_fsmManager.CreateFSM<T>(strName, owner);
        }
    }
}
