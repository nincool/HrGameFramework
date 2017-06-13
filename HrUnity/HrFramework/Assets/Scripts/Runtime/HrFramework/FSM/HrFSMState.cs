using System.Collections;
using System.Collections.Generic;

namespace Hr
{
    public abstract class HrFSMState<T> : IFSMState
    {
        protected T m_owner;

        public HrFSMState(T owner)
        {
            m_owner = owner;
        }

        public virtual void OnEnter()
        {

        }

        public virtual void OnUpdate( float fElapseSeconds, float fRealElapseSeconds)
        {

        }

        public virtual void OnExit()
        {

        }

        public virtual void OnDestroy()
        {

        }
    }

}
