using Hr.EventSystem;
using System.Collections;

namespace Hr.Scene
{
    public class HrSceneBattle : HrScene
    {
        public HrSceneBattle(HrSceneManager owner) : base(owner)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            HrLogger.Log("HrSceneBattle OnEnter!");
            HrCoroutineManager.StartCoroutine(LoadCachedSceneAndInitProcedure());
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

        protected override void AddProcedure()
        {
            m_fsmProcedureStateMachine.AddState(new Procedure.HrSceneBattle.HrProcedureInit(this));
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
            ChangeState<Procedure.HrSceneBattle.HrProcedureInit>();
        }
    }

}
