using Hr.Define;
using Hr.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.UI
{
    public class HrUIPanel01Logic : HrUILogic
    {
        private HrUIPanel01View m_panel01View;

        public HrUIPanel01Logic ()
        {
            ViewType = EnumUIViewType.UI_VIEWTYPE_NORMAL;
            ShowMode = EnumUIShowMode.UI_SHOWMODE_HIDEOTHER;
            ColliderMode = EnumUIColliderMode.UI_COLLIDER_NONE;

            RegisterEventListeners();
        }

        public override void AttachUIView(HrUIView uiView)
        {
            base.AttachUIView(uiView);

            m_panel01View = uiView as HrUIPanel01View;
        }

        public override void OnEnter()
        {

        }

        public override void OnExit()
        {

        }

        private void RegisterEventListeners()
        {
            HrGameWorld.Instance.EventComponent.AddHandler((int)EnumEventType.EVENT_UI_CLOSE_PANEL01, HandleClosePanel01View);
        }

        private void HandleClosePanel01View(object sender, HrEventHandlerArgs args)
        {
            HrGameWorld.Instance.EventComponent.SendEvent(this, new HrEventUIViewEventHandler(HrEventType.EVENT_UI_HIDE, null, ViewID));
        }
    }

}
