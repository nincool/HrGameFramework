using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.CommonUtility
{
    public class HrLoger
    {
        protected enum EnumLogType { INFO, WARNING, ERROR};

        protected static EnumLogType mEnumLogMode = EnumLogType.INFO;

        public static void Log(string strContent)
        {
            if (mEnumLogMode != EnumLogType.INFO)
            {
                return;
            }
            LogType(EnumLogType.INFO, strContent);
        }

        public static void LogWaring(string strContent)
        {
            if (mEnumLogMode == EnumLogType.INFO)
            {
                return;
            }
            LogType(EnumLogType.WARNING, strContent);
        }

        public static void LogError(string strContent)
        {
            LogType(EnumLogType.ERROR, strContent);
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
