using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Text;

namespace OdinPlugs.Wx
{
    public class WxPayApi
    {
        /// <summary>
        /// 根据当前系统时间加随机序列来生成订单号
        /// </summary>
        /// <returns>订单号</returns>
        public static string GenerateOutTradeNo()
        {
            var ran = new Random();
            return string.Format("{0}{1}{2}", WxConfigFactory.GetConfig().GetMchID(), DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));
        }


        /// <summary>
        /// 生成时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
        /// </summary>
        /// <returns>时间戳</returns>
        public static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }


        /// <summary>
        /// 生成随机串，随机串包含字母或数字
        /// </summary>
        /// <returns>随机串</returns>
        public static string GenerateNonceStr()
        {
            RandomGenerator randomGenerator = new RandomGenerator();
            return randomGenerator.GetRandomUInt().ToString();
        }


    }
}