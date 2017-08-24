using Hr.EventSystem;
using Hr.Resource;
using Hr.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Hr.Test
{
    public class HrSceneThirdPersonController : HrScene
    {
        private readonly string m_strAssetBundleName = "scene_test";

        public HrSceneThirdPersonController(HrSceneManager owner) : base(owner)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            HrLogger.Log("HrSceneThirdPersonController OnEnter!");

            //HrGameWorld.Instance.EventComponent.AddHandler(HrEventType.EVENT_LOAD_SCENE_RESOURCE_SUCCESS, HandleLoadSceneAssetBundleSuccess);
            //HrGameWorld.Instance.SceneComponent.LoadUnitySceneAssetBundleAsync(HrResourcePath.CombineStreamingAssetsPath(m_strAssetBundleName));
            SceneManager.LoadScene("Assets/Media/Scene/GameTest/HrThirdPersonController.unity", LoadSceneMode.Additive);
            SceneManager.sceneLoaded += LoadSceneSuccess;
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
            m_fsmProcedureStateMachine.AddState(new Scene.Procedure.HrSceneThirdPersonController.HrProcedureInit(this));
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
            ChangeState<Scene.Procedure.HrSceneThirdPersonController.HrProcedureInit>();

            yield return null;
        }

        private void LoadSceneSuccess(UnityEngine.SceneManagement.Scene scene, LoadSceneMode loadMode)
        {
            HrGameWorld.Instance.EventComponent.SendEvent(this, new HrEventHandlerArgs(HrEventType.EVENT_SCENE_LOADED_SCENE, null));
            ChangeState<Scene.Procedure.HrSceneThirdPersonController.HrProcedureInit>();
        }


    }

}
