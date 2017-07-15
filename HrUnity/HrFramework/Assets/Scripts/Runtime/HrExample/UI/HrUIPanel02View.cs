using Hr.Define;
using Hr.EventSystem;
using Hr.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.UI
{
    public class HrUIPanel02View : HrUIView
    {
        public override int UIID
        {
            get
            {
                return (int)EnumUIType.UITYPE_PANEL02_VIEW;
            }
        }

        public void OnClickCloseButton()
        {
            HrGameWorld.Instance.EventComponent.SendEvent(this, new HrEventHandlerArgs((int)EnumEventType.EVENT_UI_CLOSE_PANEL02, null));
        }
    }

}
