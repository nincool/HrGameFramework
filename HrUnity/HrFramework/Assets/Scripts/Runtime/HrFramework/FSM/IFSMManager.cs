using Hr;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.FSM
{
    public interface IFSMManager
    {
        IFSMStateMachine AddFSM<T>(string strName, T owner) where T : class;

        bool RemoveFSM(string strName);

    }

}
