using System;
using OdinPlugs.Wx.Models;

namespace OdinPlugs.Wx
{
    public class WxConfig : IConfig
    {
        public WxConfig() { }

        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
         */

        /// <summary>
        /// 绑定支付的APPID（必须配置）
        /// </summary>
        /// <returns></returns>
        public string GetAppID()
        {
            return "wx71297b8fc780958c";
        }
        /// <summary>
        /// 商户号（必须配置）
        /// </summary>
        /// <returns></returns>
        public string GetMchID()
        {
            return "1485492942";
        }
        /// <summary>
        /// 商户支付密钥，参考开户邮件设置（必须配置），请妥善保管，避免密钥泄露
        /// </summary>
        /// <returns></returns>
        public string GetKey()
        {
            return "GlSxHGWYdpRfZdumpygHXeghPHoEfJaI";
        }
        /// <summary>
        /// 公众帐号secert（仅JSAPI支付的时候需要配置），请妥善保管，避免密钥泄露
        /// </summary>
        /// <returns></returns>
        public string GetAppSecret()
        {
            return "7ead31cd6d58cd9199b6d23eda1dcdf0";
        }
        /// <summary>
        /// 微信服务器配置-服务器url （如果在微信公众平台配置了微信服务器配置则 必须配置）
        /// </summary>
        /// <returns></returns>
        public string GetServerUrl()
        {
            return "http://weixin.tongfutele.com/api/v1/WxMessage/Message";
        }

        public string GetJsAuthenUrl()
        {
            return "http://weixin.tongfutele.com/";
        }
        /// <summary>
        /// 微信服务器配置-Token （如果在微信公众平台配置了微信服务器配置则 必须配置）
        /// </summary>
        /// <returns></returns>
        public string GetWxToken()
        {
            return "dd8404ec0e4b085798a5477c0640ca55";
        }
        /// <summary>
        /// 微信服务器配置-消息加解密密钥 （如果在微信公众平台配置了微信服务器配置则 必须配置）
        /// </summary>
        /// <returns></returns>
        public string GetEncodingAESKey()
        {
            return "U32elc6jjk0kdYQiOYNqG0kSmLnoP1ub30ySacnxGQW";
        }
        /// <summary>
        /// 微信Api接口服务器url（必须配置，可检查测试，默认值是否可用）
        /// </summary>
        /// <returns></returns>
        public string GetWxApiServerUrl()
        {
            return $"https://api.weixin.qq.com"; ;
        }
        /// <summary>
        /// 公众号全局唯一接口调用凭据Token的ApiPath（必须配置，默认值是否可用）
        /// </summary>
        /// <returns></returns>
        public string GetWxServerAccessTokenUrlPath()
        {
            string path = "cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
            path = String.Format(path, GetAppID(), GetAppSecret());
            return path;
        }

        public AccessToken_Model WxApiAccessToken { get; set; } = null;
        public AccessToken_Model GetWxApiAccessToken()
        {
            return WxApiAccessToken;
        }

        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
         * 1.证书文件不能放在web服务器虚拟目录，应放在有访问权限控制的目录中，防止被他人下载；
         * 2.建议将证书文件名改为复杂且不容易猜测的文件
         * 3.商户服务器要做好病毒和木马防护工作，不被非法侵入者窃取证书文件。
         */
        public string GetSSlCertPath()
        {
            return "";
        }
        public string GetSSlCertPassword()
        {
            return "";
        }

        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
         */
        public string GetNotifyUrl()
        {
            return "http://weixin.tongfutele.com/api/v1/WxPay/Callback";
        }
        /// <summary>
        /// 公众号支付 支付授权目录
        /// </summary>
        /// <returns></returns>
        public string[] PayAuthorizeUrl()
        {
            return new string[] { "http://weixin.tongfutele.com/tt/", "http://weixin.tongfutele.com/#/" };
        }
        /// <summary>
        /// 扫码支付 扫码回调链接
        /// </summary>
        /// <returns></returns>
        public string QRCallBackUrl()
        {
            return "http://http://weixin.tongfutele.com/api/v1/WxPay/QRCallback/";
        }

        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动配置也可在程序中自动获取
         */
        public string GetIp()
        {
            return "0.0.0.0";
        }

        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
         */
        public string GetProxyUrl()
        {
            return "";
        }

        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
         */
        public int GetReportLevel()
        {
            return 1;
        }

        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
         */
        public int GetLogLevel()
        {
            return 1;
        }
    }
}