using System.Collections;
using System.Collections.Generic;
using OdinPlugs.Wx.Models.MAEModel;

namespace OdinPlugs.Wx
{
    public class WxMessageDic
    {
        private static volatile Dictionary<string, BaseMsg> BaseMsgDic;
        private static object syncRoot = new object();
        /// <summary>
        /// 获取wxConfig配置
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, BaseMsg> GetWxMessageQueue()
        {
            if (BaseMsgDic == null)
            {
                lock (syncRoot)
                {
                    if (BaseMsgDic == null)
                        BaseMsgDic = new Dictionary<string, BaseMsg>();
                }
            }
            return BaseMsgDic;
        }
    }
}