using Hr.Define;
using Hr.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.UI
{
    public class HrUIWorldMainLogic : HrUILogic
    {
        public HrUIWorldMainLogic()
        {
            ViewType = EnumUIViewType.UI_VIEWTYPE_NORMAL;
            ShowMode = EnumUIShowMode.UI_SHOWMODE_HIDEOTHER;
            ColliderMode = EnumUIColliderMode.UI_COLLIDER_NONE;

            RegisterEventListeners();
        }

        private void RegisterEventListeners()
        {
            HrGameWorld.Instance.EventComponent.AddHandler((int)EnumEventType.EVENT_UI_CLICK_BTN_1, HandleClickBtn1);
            HrGameWorld.Instance.EventComponent.AddHandler((int)EnumEventType.EVENT_UI_CLICK_BTN_2, HandleClickBtn2);
            HrGameWorld.Instance.EventComponent.AddHandler((int)EnumEventType.EVENT_UI_CLICK_BTN_3, HandleClickBtn3);
        }

        private void HandleClickBtn1(object sender, HrEventHandlerArgs args)
        {
            HrGameWorld.Instance.EventComponent.SendEvent(this, new HrEventUIViewEventHandler(HrEventType.EVENT_UI_SHOW, null, (int)EnumUIType.UITYPE_PANEL01_VIEW));
        }

        private void HandleClickBtn2(object sender, HrEventHandlerArgs args)
        {

        }

        private void HandleClickBtn3(object sender, HrEventHandlerArgs args)
        {
            HrGameWorld.Instance.SceneComponent.SwitchToScene<Hr.Scene.HrSceneBattleLoading>();

        }
    }
}





