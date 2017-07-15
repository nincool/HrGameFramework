using Hr.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.UI
{
    public class HrUIPanel02Logic : HrUILogic
    {
        private HrUIPanel02View m_panel02View;

        public HrUIPanel02Logic()
        {
            ViewType = EnumUIViewType.UI_VIEWTYPE_NORMAL;
            ShowMode = EnumUIShowMode.UI_SHOWMODE_HIDEOTHER;
            ColliderMode = EnumUIColliderMode.UI_COLLIDER_NONE;


        }

        public override void AttachUIView(HrUIView uiView)
        {
            base.AttachUIView(uiView);

            m_panel02View = uiView as HrUIPanel02View;
        }

        public override void OnEnter()
        {

        }

        public override void OnExit()
        {

        }
    }

}
