using Hr.Define;
using Hr.EventSystem;
using Hr.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.UI
{
    public class HrUITopBarLogic : HrUILogic
    {

        private HrUITopBarView m_topBarView;

        public HrUITopBarLogic()
        {
            ViewType = EnumUIViewType.UI_VIEWTYPE_FIXED;
            ShowMode = EnumUIShowMode.UI_SHOWMODE_DONOTHING;
            ColliderMode = EnumUIColliderMode.UI_COLLIDER_NONE;
        }

        public override void AttachUIView(HrUIView uiView)
        {
            base.AttachUIView(uiView);

            m_topBarView = uiView as HrUITopBarView;
        }

        private void RegisterEventListeners()
        {
            HrGameWorld.Instance.EventComponent.AddHandler((int)EnumEventType.EVENT_UI_TOPBAR_CLICK_SHOWMSG_BTN, HandleClickShowMsg);
            HrGameWorld.Instance.EventComponent.AddHandler((int)EnumEventType.EVENT_UI_TOPBAR_CLICK_RETURN_BTN, HandleClickReturnMsg);
        }

        private void HandleClickShowMsg(object sender, HrEventHandlerArgs args)
        {
            //HrGameWorld.Instance.EventComponent.SendEvent();
        }

        private void HandleClickReturnMsg(object sender, HrEventHandlerArgs args)
        {

        }
    }

}
