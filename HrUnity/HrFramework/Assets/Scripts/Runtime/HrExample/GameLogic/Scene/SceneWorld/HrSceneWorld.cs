using Hr.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Scene
{
    public class HrSceneWorld : HrScene
    {
        public HrSceneWorld(HrSceneManager sceneManager) :base(sceneManager)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            HrLogger.Log("HrSceneWorld OnEnter!");

            HrCoroutineManager.StartCoroutine(LoadCachedSceneAndInitProcedure());
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
            m_fsmProcedureStateMachine.AddState(new Procedure.HrSceneWorld.HrProcedureInit(this));
            m_fsmProcedureStateMachine.AddState(new Procedure.HrSceneWorld.HrProcedureMain(this));
        }

        protected override IEnumerator LoadCachedSceneAndInitProcedure()
        {
            HrGameWorld.Instance.SceneComponent.LoadUnityCachedSceneSync();

            yield return null;

            while (!HrGameWorld.Instance.SceneComponent.IsCurrentUnitySceneLoaded())
            {
                yield return null;
            }

            HrGameWorld.Instance.EventComponent.SendEvent(this, new HrEventHandlerArgs(HrEventType.EVENT_SCENE_LOADED_SCENE, null));
            ChangeState<Procedure.HrSceneWorld.HrProcedureInit>();
        }
    }

}
