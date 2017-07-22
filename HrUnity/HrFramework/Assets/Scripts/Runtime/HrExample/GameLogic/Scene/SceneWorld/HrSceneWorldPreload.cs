using Hr.EventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Scene
{
    public class HrSceneWorldPreload : HrScenePreload
    {
        private string m_strAssetBundleName = "scene_loading";

        public HrSceneWorldPreload(HrSceneManager sceneManager) : base(sceneManager)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            HrLogger.Log("HrSceneWorldPreload OnEnter!");
            HrGameWorld.Instance.EventComponent.AddHandler(HrEventType.EVENT_LOAD_SCENE_RESOURCE_SUCCESS, HandleLoadSceneAssetBundleSuccess);
            HrGameWorld.Instance.SceneComponent.LoadUnitySceneAssetBundleAsync(Resource.HrResourcePath.CombineAssetBundlePath(m_strAssetBundleName));
        }

        protected override void AddProcedure()
        {
            m_fsmProcedureStateMachine.AddState(new Procedure.HrSceneWorldPreload.HrProcedureLoading(this));
        }

        private void HandleLoadSceneAssetBundleSuccess(object sender, HrEventHandlerArgs args)
        {
            HrGameWorld.Instance.EventComponent.RemoveHandler(HrEventType.EVENT_LOAD_SCENE_RESOURCE_SUCCESS, HandleLoadSceneAssetBundleSuccess);

            HrCoroutineManager.StartCoroutine(LoadCachedSceneAndInitProcedure());
        }

        /// <summary>
        /// Unity在下一帧加载，但是又保证不了下一帧协成的执行顺序,所以先判断场景是否真正的加载成功了
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator LoadCachedSceneAndInitProcedure()
        {
            HrGameWorld.Instance.SceneComponent.LoadUnityCachedSceneSync();

            yield return null;

            while (!HrGameWorld.Instance.SceneComponent.IsCurrentUnitySceneLoaded())
            {
                yield return null;
            }

            HrGameWorld.Instance.EventComponent.SendEvent(this, new HrEventHandlerArgs(HrEventType.EVENT_SCENE_LOADED_SCENE, null));
            ChangeState<Procedure.HrSceneWorldPreload.HrProcedureLoading>();
        }
    }
}

