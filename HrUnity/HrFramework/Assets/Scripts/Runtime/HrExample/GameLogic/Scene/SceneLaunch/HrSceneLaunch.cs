
using Hr.EventSystem;
using Hr.Resource;
using System.Collections;
using UnityEngine;

namespace Hr.Scene
{
    public class HrSceneLaunch : HrScene
    {
        private  string m_strAssetBundleName = "scene_launch";

        public HrSceneLaunch(HrSceneManager sceneManager) :base(sceneManager)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            HrLogger.Log("HrSceneLaunchImp OnEnter!");

            HrGameWorld.Instance.EventComponent.AddHandler(HrEventType.EVENT_LOAD_SCENE_RESOURCE_SUCCESS, HandleLoadSceneAssetBundleSuccess);
            HrGameWorld.Instance.SceneComponent.LoadSceneAssetBundleSync(HrResourcePath.CombineStreamingAssetsPath(m_strAssetBundleName));

            ChangeState<Procedure.HrSceneLaunch.HrProcedureInit>();
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
        }

        private void HandleLoadSceneAssetBundleSuccess(object sender, HrEventHandlerArgs args)
        {
            HrGameWorld.Instance.SceneComponent.LoadCachedSceneSync();

            HrGameWorld.Instance.EventComponent.RemoveHandler(HrEventType.EVENT_LOAD_SCENE_RESOURCE_SUCCESS, HandleLoadSceneAssetBundleSuccess);
        }
    }
}
