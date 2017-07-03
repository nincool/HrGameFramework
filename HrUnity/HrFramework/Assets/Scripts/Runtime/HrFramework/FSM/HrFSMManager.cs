using System;
using System.Collections;
using System.Collections.Generic;

namespace Hr
{
    public class HrFSMManager : HrModule
    {
        private readonly Dictionary<string, IFSMStateMachine> m_dicStateMachines = new Dictionary<string, IFSMStateMachine>();

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
            m_dicStateMachines.Add(strName, fsm);

            return fsm;
        }
        
        public bool RemoveFSM(string strName)
        {
            if (m_dicStateMachines.ContainsKey(strName))
            {
                m_dicStateMachines.Remove(strName);

                return true;
            }
            return false;
        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
            foreach (var item in m_dicStateMachines.Values)
            {
                item.OnUpdate(fElapseSeconds, fRealElapseSeconds);
            }
        }

        public override void Shutdown()
        {

        }
    }
}
