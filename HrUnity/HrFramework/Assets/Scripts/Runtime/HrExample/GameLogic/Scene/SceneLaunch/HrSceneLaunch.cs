
using Hr.EventSystem;
using Hr.Resource;
using System.Collections;
using UnityEngine;

namespace Hr.Scene
{
    public class HrSceneLaunch : HrScene
    {
        private readonly string m_strAssetBundleName = "scene_launch";

        public HrSceneLaunch(HrSceneManager sceneManager) :base(sceneManager)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            HrLogger.Log("HrSceneLaunch OnEnter!");

            HrGameWorld.Instance.EventComponent.AddHandler(HrEventType.EVENT_LOAD_SCENE_RESOURCE_SUCCESS, HandleLoadSceneAssetBundleSuccess);
            HrGameWorld.Instance.SceneComponent.LoadUnitySceneAssetBundleAsync(HrResourcePath.CombineStreamingAssetsPath(m_strAssetBundleName));
        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnDestroy()
        {
        }

        protected override void AddProcedure()
        {
            m_fsmProcedureStateMachine.AddState(new Procedure.HrSceneLaunch.HrProcedureInit(this));
            m_fsmProcedureStateMachine.AddState(new Procedure.HrSceneLaunch.HrProcedureSplash(this));
            m_fsmProcedureStateMachine.AddState(new Procedure.HrSceneLaunch.HrProcedureCheckVersion(this));
            m_fsmProcedureStateMachine.AddState(new Procedure.HrSceneLaunch.HrProcedureReadyEnterGame(this));
            m_fsmProcedureStateMachine.AddState(new Procedure.HrSceneLaunch.HrProcedureTest(this));
        }

        private void HandleLoadSceneAssetBundleSuccess(object sender, HrEventHandlerArgs args)
        {
            HrGameWorld.Instance.EventComponent.RemoveHandler(HrEventType.EVENT_LOAD_SCENE_RESOURCE_SUCCESS, HandleLoadSceneAssetBundleSuccess);
            HrCoroutineManager.StartCoroutine(LoadCachedSceneAndInitProcedure());
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
            ChangeState<Procedure.HrSceneLaunch.HrProcedureInit>();
        }
    }
}
