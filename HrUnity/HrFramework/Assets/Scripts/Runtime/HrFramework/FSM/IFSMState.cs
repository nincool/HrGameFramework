using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public interface IFSMState 
    {
        void OnEnter();


        void OnUpdate(float fElapseSeconds, float fRealElapseSeconds);


        void OnExit();


        void OnDestroy();
    }

}

