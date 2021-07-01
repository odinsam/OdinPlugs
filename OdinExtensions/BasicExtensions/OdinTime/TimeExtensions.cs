
using System;
using OdinPlugs.OdinBasicDataType.OdinEnum;
using OdinPlugs.OdinUtils.OdinTime;

namespace OdinPlugs.OdinExtensions.BasicExtensions.OdinTime
{
    public static class TimeExtensions
    {
        private static readonly DateTime BaseTime = new DateTime(1970, 1, 1);
        public static long ToUnixTime(this DateTime now)
        {
            return UnixTimeHelper.GetUnixDateTime();
        }

        public static long ToUnixTimeMs(this DateTime now)
        {
            return UnixTimeHelper.GetUnixDateTimeMS();
        }

        /// <summary>
        /// 时间戳反转为时间，有很多中翻转方法，但是，请不要使用过字符串（string）进行操作，大家都知道字符串会很慢！
        /// </summary>
        /// <param name="TimeStamp">时间戳</param>
        /// <param name="AccurateToMilliseconds">是否精确到毫秒</param>
        /// <returns>返回一个日期时间</returns>
        public static DateTime ToTimer(this long TimeStamp, EnumSsOrMs? SsOrMs = EnumSsOrMs.ss)
        {
            if (SsOrMs == EnumSsOrMs.ms)
            {
                return BaseTime.ToLocalTime().AddTicks(TimeStamp * 10000);
            }
            else
            {
                return BaseTime.ToLocalTime().AddTicks(TimeStamp * 10000000);
            }
        }

        /// <summary>
        /// 时间戳反转为时间，有很多中翻转方法，但是，请不要使用过字符串（string）进行操作，大家都知道字符串会很慢！
        /// </summary>
        /// <param name="TimeStamp">时间戳</param>
        /// <param name="AccurateToMilliseconds">是否精确到毫秒</param>
        /// <returns>返回一个日期时间</returns>
        public static DateTime ToTimer(this string TimeStamp, EnumSsOrMs? SsOrMs = EnumSsOrMs.ss)
        {
            if (SsOrMs == EnumSsOrMs.ms)
            {
                return BaseTime.ToLocalTime().AddTicks(Convert.ToInt64(TimeStamp) * 10000);
            }
            else
            {
                return BaseTime.ToLocalTime().AddTicks(Convert.ToInt64(TimeStamp) * 10000000);
            }
        }

        /// <summary>
        /// 时间差返回 MM:dd HH:mm
        /// </summary>
        /// <returns></returns>
        public static string TimeDifferenceToString(this long longts, long dtNow)
        {
            TimeSpan ts = new TimeSpan(longts);
            DateTime now = dtNow.ToTimer();
            if (ts.Days > 0)
            {
                now = now.AddDays((0 - ts.Days));
                return now.ToString("MM月dd日 hh:mm");
            }
            else
                return now.ToString("hh:mm");
        }
    }
}