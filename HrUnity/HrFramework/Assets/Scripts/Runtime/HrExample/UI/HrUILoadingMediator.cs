using System.Collections;
using System.Collections.Generic;

namespace Hr
{
    public class HrUILoadingMediator : HrUIMediator
    {

        public override void Show()
        {
            if (m_view == null)
            {
                //HrEventManager.Instance.SendEvent(EnumEvent.EVENT_UI_CREATE, EnumView.VIEW_LAUNCH_LOADING);
            }
        }

    }

}
