using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hr.EventSystem
{
    public sealed class HrEventHandlerArgs : HrEventArgs
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

        public HrEventHandlerArgs(int nEvent, object UserData)
        {
            EventID = nEvent;
        }
    }
}
