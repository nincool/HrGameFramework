using System;
using System.Collections;
using System.Collections.Generic;

namespace Hr.FSM
{
    public class HrFSMManager : HrModule, IFSMManager
    {
        private readonly Dictionary<string, IFSMStateMachine> m_dicStateMachines = new Dictionary<string, IFSMStateMachine>();

        /// <summary>
        /// 删除FSM 放在下一帧Update前删除
        /// </summary>
        private List<string> m_lisRemoveFSMCache = new List<string>();
        private List<IFSMStateMachine> m_lisAddFSMCache = new List<IFSMStateMachine>();

        public override void Init()
        {

        }

        public IFSMStateMachine AddFSM<T>(string strName, T owner) where T: class
        {
            if (m_dicStateMachines.HrTryGet(strName) != null)
            {
                HrLogger.LogError(string.Format("HrFSMManager AddFSM Error! Already exist fsm:[%s]", strName));
                return null;
            }
            HrFSMStateMachine<T> fsm = new HrFSMStateMachine<T>(strName, owner);
            m_lisAddFSMCache.Add(fsm); 

            return fsm;
        }
        
        public bool RemoveFSM(string strName)
        {
            if (m_dicStateMachines.ContainsKey(strName))
            {
                m_lisRemoveFSMCache.Add(strName);
                return true;
            }
            return false;
        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
            foreach (var itemAddFSM in m_lisAddFSMCache)
            {
                m_dicStateMachines.Add(itemAddFSM.Name, itemAddFSM);
            }
            m_lisAddFSMCache.Clear();

            foreach (var itemRemoveName in m_lisRemoveFSMCache)
            {
                m_dicStateMachines.Remove(itemRemoveName);
            }
            m_lisRemoveFSMCache.Clear();

            int nStateMachineCount = m_dicStateMachines.Count;
            foreach (var item in m_dicStateMachines)
            {
                item.Value.OnUpdate(fElapseSeconds, fRealElapseSeconds);
#if UNITY_EDITOR
                if (nStateMachineCount != m_dicStateMachines.Count)
                {
                    HrLogger.LogError("FSMManager Update Error! can not add or remove stateMachine in update func!!!");
                }
#endif
            }

        }

        public override void OnUpdateEndOfFrame(float fElapseSeconds, float fRealElapseSeconds)
        {
        }

        public override void Shutdown()
        {

        }
    }
}
