using System;
using System.Collections;
using System.Collections.Generic;

namespace Hr
{
    public class HrException : Exception
    {
        public HrException() : base()
        {

        }

        public HrException(string strMessage) : base(strMessage)
        {

        }

        public HrException(string strMessage, Exception innerException) : base(strMessage, innerException)
        {

        }
    }
}

