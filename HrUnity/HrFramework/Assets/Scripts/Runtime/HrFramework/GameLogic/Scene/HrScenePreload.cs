using Hr.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Scene
{
    public class HrScenePreload : HrScene
    {
        public HrScenePreload(HrSceneManager sceneManager) :base(sceneManager)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            HrLogger.Log("HrScenePreload OnEnter!");

        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {

        }

        public override void OnExit()
        {

        }

        public override void OnDestroy()
        {

        }

        protected override void AddProcedure()
        {
        }

        protected override IEnumerator LoadCachedSceneAndInitProcedure()
        {
            yield break;
        }

    }
}
