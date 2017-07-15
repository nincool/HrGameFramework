using Hr.EventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Scene
{
    public class HrSceneWorldPreload : HrScene
    {
        private string m_strAssetBundleName = "scene_loading";

        public HrSceneWorldPreload(HrSceneManager sceneManager) : base(sceneManager)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            HrGameWorld.Instance.EventComponent.AddHandler(HrEventType.EVENT_LOAD_SCENE_RESOURCE_SUCCESS, HandleLoadSceneAssetBundleSuccess);
            HrGameWorld.Instance.SceneComponent.LoadSceneAssetBundleSync(Resource.HrResourcePath.CombineAssetBundlePath(m_strAssetBundleName));

            ChangeState<Procedure.HrSceneWorldPreload.HrProcedureLoading>();
        }

        protected override void AddProcedure()
        {
            m_fsmProcedureStateMachine.AddState(new Procedure.HrSceneWorldPreload.HrProcedureLoading(this));
        }

        private void HandleLoadSceneAssetBundleSuccess(object sender, HrEventHandlerArgs args)
        {
            HrGameWorld.Instance.SceneComponent.LoadCachedSceneSync();
            HrGameWorld.Instance.EventComponent.RemoveHandler(HrEventType.EVENT_LOAD_SCENE_RESOURCE_SUCCESS, HandleLoadSceneAssetBundleSuccess);
        }
    }
}

