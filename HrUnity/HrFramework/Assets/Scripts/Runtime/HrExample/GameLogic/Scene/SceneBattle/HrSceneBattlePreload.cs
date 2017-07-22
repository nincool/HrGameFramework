using Hr.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Scene
{
    public class HrSceneBattlePreload : HrScenePreload
    {
        private string m_strAssetBundleName = "scene_loading";

        public HrSceneBattlePreload(HrSceneManager sceneManager) :base(sceneManager)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            HrGameWorld.Instance.EventComponent.AddHandler(HrEventType.EVENT_LOAD_SCENE_RESOURCE_SUCCESS, HandleLoadSceneAssetBundleSuccess);
            HrGameWorld.Instance.SceneComponent.LoadUnitySceneAssetBundleSync(Resource.HrResourcePath.CombineAssetBundlePath(m_strAssetBundleName));
        }

        protected override void AddProcedure()
        {
            m_fsmProcedureStateMachine.AddState(new Procedure.HrSceneBattlePreload.HrProcedurePreload(this));
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
            ChangeState<Procedure.HrSceneBattlePreload.HrProcedurePreload>();
        }

        private void HandleLoadSceneAssetBundleSuccess(object sender, HrEventHandlerArgs args)
        {
            HrGameWorld.Instance.EventComponent.RemoveHandler(HrEventType.EVENT_LOAD_SCENE_RESOURCE_SUCCESS, HandleLoadSceneAssetBundleSuccess);

            HrCoroutineManager.StartCoroutine(LoadCachedSceneAndInitProcedure());
        }
    }
}
