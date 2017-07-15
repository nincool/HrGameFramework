using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hr.EventSystem
{
    public class HrEventHandlerArgs : HrEventArgs
    {
        public int EventID
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public HrEventHandlerArgs(int nEvent, object data)
        {
            EventID = nEvent;
            UserData = data;
        }
    }

    public class HrEventLoadSceneResourceEventHandler : HrEventHandlerArgs
    {
        public HrEventLoadSceneResourceEventHandler(int nEvent, object data) : base(nEvent, data)
        {

        }
    }

    public class HrEventUIViewEventHandler : HrEventHandlerArgs
    {
        public int PanelType
        {
            get;
            set;
        }

        public HrEventUIViewEventHandler(int nEvent, object data, int nPanelType) : base(nEvent, data)
        {
            PanelType = nPanelType;
        }
    }
}
