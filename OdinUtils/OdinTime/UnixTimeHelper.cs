using System;
using OdinPlugs.OdinBasicDataType.OdinEnum;

namespace OdinPlugs.OdinUtils.OdinTime
{
    public class UnixTimeHelper
    {
        private static readonly DateTime BaseTime = new DateTime(1970, 1, 1);
        /// <summary>    
        /// 将unixtime转换为.NET的DateTime    
        /// </summary>    
        /// <param name="timeStamp">秒数</param>    
        /// <returns>转换后的时间</returns>    
        public static long FromDateTime(DateTime dateTime, EnumSsOrMs? SsOrMs = EnumSsOrMs.ss)
        {
            if (SsOrMs == null || SsOrMs == EnumSsOrMs.ss)
                return FromDateTime(dateTime);
            else
                return (dateTime.ToLocalTime().Ticks - BaseTime.ToLocalTime().Ticks) / 10000;
        }

        /// <summary>    
        /// 将.NET的DateTime转换为unix time    
        /// </summary>    
        /// <param name="dateTime">待转换的时间</param>    
        /// <returns>转换后的unix time</returns>    
        public static long FromDateTime(DateTime dateTime)
        {
            return (dateTime.ToLocalTime().Ticks - BaseTime.ToLocalTime().Ticks) / 10000000;
        }

        /// <summary>    
        /// 将.NET的DateTime转换为unix time    
        /// </summary>    
        /// <returns>转换后的unix time</returns>    
        public static long GetUnixDateTime() => FromDateTime(DateTime.Now);
        /// <summary>    
        /// 将.NET的DateTime转换为unix time ms
        /// </summary>    
        /// <returns>转换后的unix time</returns>    
        public static long GetUnixDateTimeMS() => FromDateTime(DateTime.Now, EnumSsOrMs.ms);
    }
}