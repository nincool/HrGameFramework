using Hr.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public sealed class HrFSMComponent : HrComponent
    {
        public bool InitSuccess { get; private set; }

        private IFSMManager m_fsmManager = null;

        protected override void Awake()
        {
            base.Awake();

            m_fsmManager = HrGameWorld.Instance.GetModule<IFSMManager>();
            if (m_fsmManager != null)
                InitSuccess = true;
        }

        public IFSMStateMachine AddFSM<T>(string strName, T owner) where T : class
        {
            return m_fsmManager.AddFSM<T>(strName, owner);
        }

        public bool RemoveFSM(string strName)
        {
            return m_fsmManager.RemoveFSM(strName);
        }
    }
}
