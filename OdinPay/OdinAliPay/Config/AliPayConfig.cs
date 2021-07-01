using System.Collections.Generic;
using System.IO;
using OdinPlugs.OdinCore.ConfigModel.PayConfigModel;

namespace OdinPlugs.OdinPay.OdinAliPay.Config
{
    public enum AliPayTypeEnum
    {
        /// <summary>
        /// app支付
        /// </summary>
        AppPay,
        /// <summary>
        /// 手机网站支付
        /// </summary>
        AppWebPay,
        /// <summary>
        /// 电脑网站支付
        /// </summary>
        PcWebPay,
        /// <summary>
        /// 当面付
        /// </summary>
        FacePay,
    }
    public class AliPayConfig
    {

        #region 配置信息
        /// <summary>
        /// 应用公钥
        /// </summary>
        public string AppPublicKey { get; set; }
        /// <summary>
        /// 支付宝公钥
        /// </summary>
        public string AlipayPublicKey { get; set; }
        /// <summary>
        /// 应用ID,您的APPID 
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 编码格式
        /// </summary>
        public string CharSet { get; set; }
        /// <summary>
        /// 支付宝网关
        /// </summary>
        public string Gatewayurl { get; set; }
        /// <summary>
        /// 商户私钥，您的原始格式RSA私钥
        /// </summary>
        public string PrivateKey { get; set; }
        /// <summary>
        /// 签名方式
        /// </summary>
        public string SignType { get; set; }
        /// <summary>
        /// 服务器异步通知页面路径 Post访问
        /// </summary>
        public string AliNotifyUrl { get; set; }
        /// <summary>
        /// 页面跳转同步通知页面路径  Get访问
        /// </summary>
        public string AliReturnUrl { get; set; }
        /// <summary>
        /// 参数返回格式，只支持json
        /// </summary>
        public string AliFormat { get; set; } = "json";
        /// <summary>
        /// 商户ID
        /// </summary>
        public string Uid { get; set; }
        /// <summary>
        /// 支付宝SDK版本
        /// </summary>
        /// <value></value>
        public string Version { get; set; } = "2.0";

        public CertPath_Model CertPath { get; set; }

        #endregion

        public AliPayConfig(AliPayCnfOptions aliPayConfig)
        {
            this.AppPublicKey = aliPayConfig.AppPublicKey;
            this.AlipayPublicKey = aliPayConfig.AlipayPublicKey;
            this.AppId = aliPayConfig.AppId;
            this.CharSet = aliPayConfig.CharSet;
            this.Gatewayurl = aliPayConfig.Gatewayurl;
            this.PrivateKey = aliPayConfig.PrivateKey;
            this.SignType = aliPayConfig.SignType;
            this.AliNotifyUrl = aliPayConfig.AliNotifyUrl;
            this.AliReturnUrl = aliPayConfig.AliReturnUrl;
            this.AliFormat = aliPayConfig.AliFormat;
            this.Uid = aliPayConfig.Uid;
            this.CertPath = new CertPath_Model
            {
                AlipayPublicKeyPath = Path.Combine(Directory.GetCurrentDirectory(), aliPayConfig.CertPath.AlipayPublicKeyPath),
                AppPublicKeyPath = Path.Combine(Directory.GetCurrentDirectory(), aliPayConfig.CertPath.AppPublicKeyPath),
                AliRootPath = Path.Combine(Directory.GetCurrentDirectory(), aliPayConfig.CertPath.AliRootPath),
            };
        }

    }
}