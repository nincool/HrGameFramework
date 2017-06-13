
namespace Hr.Scene
{
    public class HrSceneLaunch : HrScene
    {
        private const string m_c_strLaunchSceneName = "Assets/Media/Scene/GameExample/Launch";
        private const string m_c_strAssetBundleName = "scene_launch";

        public HrSceneLaunch(HrSceneManager sceneManager) :base(sceneManager)
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();
            HrLogger.Log("HrSceneLaunchImp OnEnter!");

            HrGameWorld.Instance.ResourceComponent.LoadSceneSync(m_c_strLaunchSceneName, m_c_strAssetBundleName);

            ChangeState<Procedure.HrSceneLaunch.HrProcedureInit>();
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
    }
}
