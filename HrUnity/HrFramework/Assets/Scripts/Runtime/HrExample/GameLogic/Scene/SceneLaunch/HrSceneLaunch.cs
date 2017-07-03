
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

            HrGameWorld.Instance.SceneComponent.LoadSceneSync(HrResourcePath.CombineStreamingAssetsPath(m_strAssetBundleName));

            ChangeState<Procedure.HrSceneLaunch.HrProcedureInit>();

            //HrCoroutineManager.StartCoroutine(DelayChangeScene());
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
            m_fsmProcedureStateMachine.AddState(new Procedure.HrSceneLaunch.HrProcedureInit(this));
            m_fsmProcedureStateMachine.AddState(new Procedure.HrSceneLaunch.HrProcedureSplash(this));
            m_fsmProcedureStateMachine.AddState(new Procedure.HrSceneLaunch.HrProcedureCheckVersion(this));
            m_fsmProcedureStateMachine.AddState(new Procedure.HrSceneLaunch.HrProcedurePreload(this));
        }

        //private IEnumerator DelayChangeScene()
        //{
        //    yield return new WaitForSecondsRealtime(10f);

        //}
    }
}
