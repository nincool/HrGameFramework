using Hr.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.UI
{
    public class HrUITopBarView : HrUIView
    {

        public override int UIID
        {
            get
            {
                return (int)EnumUIType.UITYPE_TOPBAR_VIEW;
            }
        }

        public void OnClickReturnButton()
        {

        }

        public void OnClickShowMsgButton()
        {

        }
    }
}
