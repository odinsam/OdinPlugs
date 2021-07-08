using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OdinPlugs.OdinMvcCore.OdinInject;
using OdinPlugs.OdinNetCore.WebApi.HttpClientHelper;
using OdinPlugs.OdinNetCore.WebApi.HttpClientHelper.HttpClientInterface;
using OdinPlugs.OdinUtils.Utils.OdinTime;
using OdinPlugs.Wx.Models;
using OdinPlugs.Wx.Models.MAEModel.EventModel;
using OdinPlugs.Wx.Models.MAEModel.MessageModel;
using OdinPlugs.Wx.WxSecurity;

namespace OdinPlugs.Wx
{
    public class WxTools
    {
        public WxTools()
        {
            if (WxConfigFactory.GetConfig().WxApiAccessToken == null)
            {
                var url_getAccessToken = WxConfigFactory.GetConfig().GetWxApiServerUrl();
                var apiPath_getAccessToken = WxConfigFactory.GetConfig().GetWxServerAccessTokenUrlPath();
                var odinHttpClientFactory = OdinInjectHelper.GetService<IOdinHttpClientFactory>();
                var result = odinHttpClientFactory.GetRequestAsync<AccessToken_Model>("OdinClient", url_getAccessToken + apiPath_getAccessToken);
                AccessToken_Model accessToken = result.Result;
                WxConfigFactory.GetConfig().WxApiAccessToken = accessToken;
                if (result.Result.errcode == 0)
                {
                    accessToken.createTime = UnixTimeHelper.GetUnixDateTime();
                }
            }
        }

        #region public-GetAccessToken-获取调用wx接口所需要的accessToken
        public AccessToken_Model GetAccessToken()
        {
            return WxConfigFactory.GetConfig().WxApiAccessToken;
        }
        #endregion

        #region xml转换对应消息对象
        /// <summary>
        /// xml转换对应消息对象
        /// </summary>
        /// <param name="xmlstr">wx-xml消息内容</param>
        /// <typeparam name="T">需要装换的对象</typeparam>
        /// <returns>装换后的对象</returns>
        public static T ConvertObj<T>(string xmlstr)
        {
            XElement xdoc = XElement.Parse(xmlstr);
            var type = typeof(T);
            var t = Activator.CreateInstance<T>();
            foreach (XElement element in xdoc.Elements())
            {
                var pr = type.GetProperty(element.Name.ToString());
                if (pr != null)
                {
                    if (element.HasElements)
                    {//这里主要是兼容微信新添加的菜单类型。nnd，竟然有子属性，所以这里就做了个子属性的处理
                        foreach (var ele in element.Elements())
                        {
                            pr = type.GetProperty(ele.Name.ToString());
                            pr.SetValue(t, Convert.ChangeType(ele.Value, pr.PropertyType), null);
                        }
                        continue;
                    }
                    if (pr.PropertyType.Name == "MsgEnumType")//获取消息模型
                    {
                        pr.SetValue(t, (MsgEnumType)Enum.Parse(typeof(MsgEnumType), element.Value.ToUpper()), null);
                        continue;
                    }
                    if (pr.PropertyType.Name == "EventEnumType")//获取事件类型。
                    {
                        pr.SetValue(t, (EventEnumType)Enum.Parse(typeof(EventEnumType), element.Value.ToUpper()), null);
                        continue;
                    }
                    pr.SetValue(t, Convert.ChangeType(element.Value, pr.PropertyType), null);
                }
            }
            return t;
        }

        #endregion


        #region 微信消息检查
        /// <summary>
        /// 微信消息检查
        /// </summary>
        /// <param name="signature">微信加密签名，signature结合了开发者填写的token参数和请求中的timestamp参数、nonce参数。</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="echostr">随机字符串</param>
        /// <returns></returns>
        public bool WeChatCheck(string signature, string timestamp, string nonce, string echostr)
        {
            var token = WxConfigFactory.GetConfig().GetWxToken();
            string[] ArrTmp = { token, timestamp, nonce };
            //字典排序
            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);
            //字符加密
            var sha1 = WxSha1Sign(tmpStr);
            return sha1.Equals(signature);
        }

        #endregion


        #region 解密微信推送的消息
        public int GetPostMessage(string msg_signature, string timestamp,
                                    string nonce, string strParams, ref string decryptMsg)
        {
            WXBizMsgCrypt crypt = new WXBizMsgCrypt();
            int value = crypt.DecryptMsg(
                sMsgSignature: msg_signature,
                sTimeStamp: timestamp,
                sNonce: nonce,
                sPostData: strParams,
                sMsg: ref decryptMsg);
            return value;
        }

        #endregion


        #region 加密即将发送的wx消息
        public int EncryptMsg(string replyMsg, string timeStamp, string nonce, ref string encryptMsg)
        {
            WXBizMsgCrypt crypt = new WXBizMsgCrypt();
            int value = crypt.EncryptMsg(
                sReplyMsg: replyMsg,
                sTimeStamp: timeStamp,
                sNonce: nonce,
                sEncryptMsg: ref encryptMsg);
            return value;
        }

        #endregion


        #region HMAC-SHA1加密算法
        /// <summary>
        /// HMAC-SHA1加密算法
        /// </summary>
        /// <param name="str">加密字符串</param>
        /// <returns></returns>
        public static string WxSha1Sign(string str)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.Default.GetBytes(str));
            string byte2String = null;
            for (int i = 0; i < hash.Length; i++)
            {
                byte2String += hash[i].ToString("x2");
            }
            return byte2String;
        }

        #endregion
    }
}