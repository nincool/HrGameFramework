using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public class HrLaunchScene : HrScene
    {
        public override void OnEnter(HrSceneManager owner)
        {
            HrLogger.Log("HrLanuchScene OnEnter!");
            CreateLoadingUI();
        }

        public override void OnUpdate(HrSceneManager owner, float fElapseSeconds, float fRealElapseSeconds)
        {

        }

        public override void OnExit(HrSceneManager owner)
        {

        }

        public override void OnDestroy(HrSceneManager owner)
        {

        }

        private void CreateLoadingUI()
        {
            //HrEventManager.Instance.SendEvent(HrEventType.EVENT_UI_SHOW, EnumView.VIEW_LAUNCH_LOADING);
            HrGameWorld.Instance.EventComponent.SendEvent(HrEventType.EVENT_UI_SHOW, EnumView.VIEW_LAUNCH_LOADING);
        }
    }
}
