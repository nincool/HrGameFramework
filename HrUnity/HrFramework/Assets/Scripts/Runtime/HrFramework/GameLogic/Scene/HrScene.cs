using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Scene
{

    public abstract class HrScene : HrFSMState<HrSceneManager>
    {
        /// <summary>
        /// 用来切换每个Scene的Procedure
        /// </summary>
        protected IFSMStateMachine m_fsmProcedureStateMachine = null;

        public IFSMStateMachine FSMProcedureStateMachine
        {
            get
            {
                return m_fsmProcedureStateMachine;
            }
        }

        public HrScene(HrSceneManager owner) : base(owner)
        {
        }

        public override void OnEnter()
        {
            m_fsmProcedureStateMachine = HrGameWorld.Instance.FSMComponent.AddFSM<HrScene>(this.GetType().FullName, this) as HrFSMStateMachine<HrScene>;

            AddProcedure();
        }

        public override void OnUpdate( float fElapseSeconds, float fRealElapseSeconds)
        {
        }

        public override void OnExit()
        {
            HrGameWorld.Instance.FSMComponent.RemoveFSM(this.GetType().FullName);
        }

        public override void OnDestroy()
        {
        }
        public void ChangeState<StateType>()
        {
            ChangeState(typeof(StateType));
        }

        protected abstract void AddProcedure();

        protected void ChangeState(Type stateType)
        {
            m_fsmProcedureStateMachine.ChangeState(stateType);
        }
    }

}
