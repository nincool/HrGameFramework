using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public interface IFSMStateMachine
    {
        void Update(float fElapseSeconds, float fRealElapseSeconds);

        void Shutdown();
    }

}
