using System;
using OdinPlugs.Wx.Models;

namespace OdinPlugs.Wx
{
    public interface IConfig
    {

        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置），请妥善保管，避免密钥泄露
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置），请妥善保管，避免密钥泄露
        * ServerUrl:微信服务器配置-服务器url （如果在微信公众平台配置了微信服务器配置则 必须配置）
        * WxToken: 微信服务器配置-Token （如果在微信公众平台配置了微信服务器配置则 必须配置）
        * EncodingAESKey: 微信服务器配置-消息加解密密钥 （如果在微信公众平台配置了微信服务器配置则 必须配置）
        * WxApiServerUrl: 微信Api接口服务器url（必须配置，可检查测试，默认值是否可用）
        * WxServerAccessTokenUrlPath: 公众号全局唯一接口调用凭据Token的ApiPath（必须配置，默认值是否可用）
        */
        /// <summary>
        /// APPID：绑定支付的APPID（必须配置）
        /// </summary>
        /// <returns></returns>
        string GetAppID();
        /// <summary>
        /// MCHID：商户号（必须配置）
        /// </summary>
        /// <returns></returns>
        string GetMchID();
        /// <summary>
        /// KEY：商户支付密钥，参考开户邮件设置（必须配置），请妥善保管，避免密钥泄露
        /// </summary>
        /// <returns></returns>
        string GetKey();
        /// <summary>
        /// APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置），请妥善保管，避免密钥泄露
        /// </summary>
        /// <returns></returns>
        string GetAppSecret();
        /// <summary>
        /// ServerUrl:微信服务器配置-服务器url （如果在微信公众平台配置了微信服务器配置则 必须配置）
        /// </summary>
        /// <returns></returns>
        string GetServerUrl();
        /// <summary>
        /// WxToken: 微信服务器配置-Token （如果在微信公众平台配置了微信服务器配置则 必须配置）
        /// </summary>
        /// <returns></returns>
        string GetWxToken();
        /// <summary>
        /// EncodingAESKey: 微信服务器配置-消息加解密密钥 （如果在微信公众平台配置了微信服务器配置则 必须配置）
        /// </summary>
        /// <returns></returns>
        string GetEncodingAESKey();
        /// <summary>
        /// WxApiServerUrl: 微信Api接口服务器url（必须配置，可检查测试，默认值是否可用）
        /// </summary>
        /// <returns></returns>
        string GetWxApiServerUrl();
        /// <summary>
        /// WxServerAccessTokenUrlPath: 公众号全局唯一接口调用凭据Token的ApiPath（必须配置，默认值是否可用）
        /// </summary>
        /// <returns></returns>
        string GetWxServerAccessTokenUrlPath();
        /// <summary>
        /// WxApiAccessToken属性: 公众号全局唯一接口调用凭据Token（如需调用微信服务器接口必须使用wxtool初始化生成）
        /// </summary>
        /// <returns></returns>
        AccessToken_Model WxApiAccessToken { get; set; }
        /// <summary>
        /// WxApiAccessToken方法: 公众号全局唯一接口调用凭据Token（如需调用微信服务器接口必须使用wxtool初始化生成）
        /// </summary>
        /// <returns></returns>
        AccessToken_Model GetWxApiAccessToken();


        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
         * 1.证书文件不能放在web服务器虚拟目录，应放在有访问权限控制的目录中，防止被他人下载；
         * 2.建议将证书文件名改为复杂且不容易猜测的文件
         * 3.商户服务器要做好病毒和木马防护工作，不被非法侵入者窃取证书文件。
        */
        string GetSSlCertPath();
        string GetSSlCertPassword();



        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
        */
        string GetNotifyUrl();
        /// <summary>
        /// 公众号支付 支付授权目录
        /// </summary>
        /// <returns></returns>
        string[] PayAuthorizeUrl();
        /// <summary>
        /// 扫码支付 扫码回调链接
        /// </summary>
        /// <returns></returns>
        string QRCallBackUrl();



        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动配置也可在程序中自动获取
        */
        string GetIp();


        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        string GetProxyUrl();


        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
        int GetReportLevel();


        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        int GetLogLevel();


    }
}
