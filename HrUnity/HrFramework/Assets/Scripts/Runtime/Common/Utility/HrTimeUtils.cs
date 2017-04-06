using System;
using System.Collections.Generic;
using System.Globalization;

namespace Hr.CommonUtility
{
    public class HrTimeUtils 
    {
        public const long TICKS_PER_MILLISECOND = 10000;
        public const float TICKS_PER_SECOND = 10000000.0f;

        public static string DateTime2DateStr(DateTime dt)
        {
            return string.Format("{0:yyyy-MM-dd}", dt);
        }

        public static string DateTime2DateTimeStr(DateTime dt)
        {
            return string.Format("{0:yyyy-MM-dd HH:mm:ss}", dt);
        }

        public static DateTime DateTimeStr2DateTime(string str)
        {
            return DateTime.Parse(str);
        }

        public static string FormatBy_HHMMSS(int seconds)
        {
            return FormatBy_HHMMSS(seconds, true);
        }

        public static string FormatBy_HHMMSS(int seconds, bool hasHour)
        {
            if (seconds < 0)
            {
                seconds = 0;
            }
            int num = seconds / 0xe10;
            int num2 = (seconds % 0xe10) / 60;
            seconds = (seconds % 0xe10) % 60;
            if (hasHour)
            {
                string[] textArray1 = new string[] { num.ToString("D2"), ":", num2.ToString("D2"), ":", seconds.ToString("D2") };
                return string.Concat(textArray1);
            }

            return (num2.ToString("D2") + ":" + seconds.ToString("D2"));
        }

        public static long GetClientTimeInMilliseconds()
        {

            return (DateTime.Now.Ticks / TICKS_PER_MILLISECOND);
        }

        public static float GetClinetTimeInSeconds()
        {
            return (DateTime.Now.Ticks / TICKS_PER_SECOND);
        }

        public static DateTime GetDateTimeByHHMM(string str)
        {
            DateTimeFormatInfo provider = new DateTimeFormatInfo
            {
                ShortTimePattern = "HH:mm"
            };

            return Convert.ToDateTime(str, provider);
        }

        public static string GetRemainTime(int sec, bool allow_zero)
        {
            if (sec < 0)
            {
                if (!allow_zero)
                {
                    return null;
                }
                sec = 0;
            }
            int num = sec / 0xe10;
            int num2 = (sec % 0xe10) / 60;
            sec = (sec % 0xe10) % 60;
            if (num > 0)
            {
                return string.Format("{0:D2}:{1:D2}:{2:D2}", num, num2, sec);
            }

            return string.Format("{0:D2}:{1:D2}", num2, sec);
        }

        public static string GetRemainTime(string time_end, bool allow_zero)
        {
            DateTime time = DateTime.Parse(time_end);
            DateTime currentServerDateTime = DateTime.Now;
            TimeSpan span = (TimeSpan)(time - currentServerDateTime);
            int nTotalSeconds = (int)span.TotalSeconds;

            return GetRemainTime(nTotalSeconds, allow_zero);
        }
    }

}
