using System.Collections;
using System.Collections.Generic;

namespace Hr
{
    public abstract class HrFSMState<T> where T : class
    {

        public virtual void OnEnter(T owner)
        {

        }

        public virtual void OnUpdate(T owner, float fElapseSeconds, float fRealElapseSeconds)
        {

        }

        public virtual void OnExit(T owner)
        {

        }

        public virtual void OnDestroy(T owner)
        {

        }
    }

}
