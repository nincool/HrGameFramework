using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public interface IFSMState 
    {
        void OnEnter(IFSMStateMachine fsm);


        void OnUpdate(IFSMStateMachine fsm, float fElapseSeconds, float fRealElapseSeconds);


        void OnExit(IFSMStateMachine fsm);


        void OnDestroy(IFSMStateMachine fsm);
    }

}

