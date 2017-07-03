using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public class HrLogger
    {
        protected enum EnumLogType { INFO, WARNING, ERROR};

        protected static EnumLogType m_enumLogMode = EnumLogType.INFO;

        public static void Log(string strContent)
        {
            if (m_enumLogMode != EnumLogType.INFO)
            {
                return;
            }
            Debug.Log(strContent);

            //LogType(EnumLogType.INFO, strContent);
        }

        public static void LogWarning(string strContent)
        {
            if (m_enumLogMode == EnumLogType.INFO)
            {
                return;
            }
            Debug.LogWarning(strContent);
            
            //LogType(EnumLogType.WARNING, strContent);
        }

        public static void LogError(string strContent)
        {
            Debug.LogError(strContent);
            
            //LogType(EnumLogType.ERROR, strContent);
        }

        protected static void LogType(EnumLogType type, string strContent)
        {
            switch (type)
            {
                case EnumLogType.INFO:
                    Debug.Log(strContent);
                    break;
                case EnumLogType.WARNING:
                    Debug.LogWarning(strContent);
                    break;
                case EnumLogType.ERROR:
                    Debug.LogError(strContent);
                    break;
            }

        }
    }

}
