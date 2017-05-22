using System;
using System.Collections;
using System.Collections.Generic;

namespace Hr
{
    public class HrFSMStateMachine<T> : IFSMStateMachine where T : class
    {
        /// <summary>
        /// 状态机名称
        /// </summary>
        private readonly string m_strName;
        /// <summary>
        /// 状态机持有者
        /// </summary>
        private readonly T m_owner;
        /// <summary>
        /// 所有状态
        /// </summary>
        private readonly Dictionary<string, HrFSMState<T>> m_dicStates;
        /// <summary>
        /// 当前状态
        /// </summary>
        private HrFSMState<T> m_currentState = null;

        public HrFSMStateMachine(string strName, T owner)
        {
            m_strName = strName ?? string.Empty;
            if (owner == null)
            {
                HrLogger.LogError("FSM owner is invalid.");
            }
            m_owner = owner;
            m_dicStates = new Dictionary<string, HrFSMState<T>>();
        }

        public HrFSMStateMachine(string strName, T owner, params HrFSMState<T>[] states)
        {
            m_strName = strName ?? string.Empty;
            if (owner == null)
            {
                HrLogger.LogError("FSM owner is invalid.");
            }
            if (states == null || states.Length < 1)
            {
                HrLogger.LogError("FSM states is invalid.");
            }
            m_owner = owner;
            m_dicStates = new Dictionary<string, HrFSMState<T>>();

            foreach (var state in states)
            {
                if (states == null)
                {
                    HrLogger.LogError("FSM states is invalid.");
                }
                string strStateName = state.GetType().FullName;
                if (m_dicStates.ContainsKey(strStateName))
                {
                    HrLogger.LogError(string.Format("FSM '{0}' state '{1}' is already exist.", strName, strStateName));
                }
                m_dicStates.Add(strStateName, state);
            }
        }

        public string Name
        {
            get
            {
                return m_strName;
            }
        }

        public T Owner
        {
            get
            {
                return m_owner;
            }
        }

        public int FSMStateCount
        {
            get
            {
                return m_dicStates.Count;
            }
        }

        public HrFSMState<T> CurrentState
        {
            get
            {
                return m_currentState;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="fElapseSeconds">逻辑流逝时间</param>
        /// <param name="fRealElapseSeconds">真实流逝时间</param>
        public void Update(float fElapseSeconds, float fRealElapseSeconds)
        {
            if (m_currentState == null)
            {
                return;
            }
            m_currentState.OnUpdate(m_owner, fElapseSeconds, fRealElapseSeconds);
        }

        public void Shutdown()
        {
            if (m_currentState != null)
            {
                m_currentState.OnExit(m_owner);
                m_currentState = null;
            }

            foreach (var state in m_dicStates)
            {
                state.Value.OnDestroy(m_owner);
            }
            m_dicStates.Clear();
        }

        public void ChangeState<TState>() where TState : HrFSMState<T>
        {
            ChangeState(typeof(TState));
        } 

        public void ChangeState(Type stateType)
        {
            HrFSMState<T> state = GetState(stateType);
            if (state == null)
            {
                HrLogger.LogError(string.Format("FSM '{0}' can not change state to '{1}' whitch is not exist.", m_strName, stateType.FullName));
            }
            if (m_currentState != null)
            {
                m_currentState.OnExit(m_owner);
            }
            m_currentState = state;
            m_currentState.OnEnter(m_owner);
        }
        
        public void AddState(HrFSMState<T> state)
        {
            string strStateName = state.GetType().FullName;
            if (m_dicStates.ContainsKey(strStateName))
            {
                HrLogger.LogError(string.Format("FSM '{0}' state '{1}' is already exist.", m_strName, strStateName));
                return;
            }

            m_dicStates.Add(strStateName, state);
        }

        public HrFSMState<T> GetState(Type stateType)
        {
            HrFSMState<T> state = m_dicStates.HrTryGet(stateType.FullName);

            return state;
        }
    }
}
