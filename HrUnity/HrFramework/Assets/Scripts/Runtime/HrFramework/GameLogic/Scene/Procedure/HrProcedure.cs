using Hr.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Scene.Procedure
{
    public abstract class HrProcedure : HrFSMState<HrScene>, IProcedure
    {
        public HrProcedure(HrScene owner) : base(owner)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
            base.OnUpdate(fElapseSeconds, fRealElapseSeconds);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }

}
