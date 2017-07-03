﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Scene.Procedure.HrSceneWorld
{
    public class HrProcedureInit : HrProcedure
    {

        public HrProcedureInit(HrScene owner) : base(owner)
        {

        }
        public override void OnEnter()
        {
            base.OnEnter();
            HrLogger.Log("HrSceneWorld.HrProcedureInit.OnEnter!");

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
