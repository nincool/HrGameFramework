using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hr.EventSystem
{
    public sealed class HrEvent
    {
        private readonly object m_sender;
        private readonly HrEventHandlerArgs m_eventArgs;

        public HrEvent(object sender, HrEventHandlerArgs args)
        {
            m_sender = sender;
            m_eventArgs = args;
        }

        public object Sender
        {
            get
            {
                return m_sender;
            }
        }

        public HrEventHandlerArgs EventArgs
        {
            get
            {
                return m_eventArgs;
            }
        }
    }
}
