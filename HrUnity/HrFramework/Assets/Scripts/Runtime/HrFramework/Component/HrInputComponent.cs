using com.ootii.Input;
using Hr.EventSystem;
using Hr.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public sealed class HrInputComponent : HrComponent
    {

        public bool InitSuccess { get; private set; }

        private IInputManager m_inputManager = null;
        private IEventManager m_eventManager = null;


        protected override void Awake()
        {
            base.Awake();

            m_inputManager = HrGameWorld.Instance.GetModule<IInputManager>();
            m_eventManager = HrGameWorld.Instance.GetModule<IEventManager>();
            if (m_inputManager != null && m_eventManager != null)
            {
                InitSuccess = true;
            }
        }

        protected override void Start()
        {
            base.Start();

            AddAlias("Left", EnumInput.A);
            AddAlias("Right", EnumInput.D);
            AddAlias("Forward", EnumInput.W);
            AddAlias("Back", EnumInput.S);
        }

        public void AddAlias(string strAlias, int nPrimaryID)
        {
            m_inputManager.AddAlias(strAlias, nPrimaryID);
        }

        public void AddAlias(string strAlias, int nPrimaryID, int nSupportID)
        {
            m_inputManager.AddAlias(strAlias, nPrimaryID, nSupportID);
        }

        public bool IsPressed(string strAlias)
        {
            return m_inputManager.IsPressed(strAlias);
        }

        public float MouseXDelta()
        {
            return m_inputManager.MouseXDelta();
        }

        public float MouseYDelta()
        {
            return m_inputManager.MouseYDelta();
        }

        public float MouseAxisX()
        {
            return m_inputManager.MouseAxisX();
        }

        public float MouseAxisY()
        {
            return m_inputManager.MouseAxisY();
        }
    }
}
