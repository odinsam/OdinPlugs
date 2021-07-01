using System;
using System.Collections.Generic;
using System.Web;

namespace OdinPlugs.Wx
{
    /**
    * 	配置账号信息
    */
    public class WxConfigFactory
    {
        private static volatile IConfig config;
        private static object syncRoot = new object();
        /// <summary>
        /// 获取wxConfig配置
        /// </summary>
        /// <returns></returns>
        public static IConfig GetConfig()
        {
            if (config == null)
            {
                lock (syncRoot)
                {
                    if (config == null)
                        config = new WxConfig();
                }
            }
            return config;
        }
    }
}