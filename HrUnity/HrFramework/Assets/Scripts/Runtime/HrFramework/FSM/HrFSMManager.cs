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

        public IFSMStateMachine CreateFSM<T>(string strName, T owner) where T: class
        {
            if (m_dicStateMachines.HrTryGet(strName) != null)
            {
                HrLogger.LogError(string.Format("HrFSMManager CreateFSM Error! Already exist fsm:[%s]", strName));
                return null;
            }
            HrFSMStateMachine<T> fsm = new HrFSMStateMachine<T>(strName, owner);
            m_dicStateMachines.Add(strName, fsm);

            return fsm;
        }

        public override void Update(float fElapseSeconds, float fRealElapseSeconds)
        {

        }
    }
}
