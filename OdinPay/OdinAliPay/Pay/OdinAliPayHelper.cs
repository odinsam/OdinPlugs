using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Newtonsoft.Json;
using OdinPlugs.OdinCore.ConfigModel.PayConfigModel;
using OdinPlugs.OdinPay.OdinAliPay.Config;

namespace OdinPlugs.OdinPay.OdinAliPay.Pay
{
    public class OdinAliPayHelper
    {
        public static AliPayConfig PayConfig { get; set; }
        private AopResponse aopResponse;
        private static volatile DefaultAopClient client;
        private AlipayTradeAppPayRequest appPayRequest;
        private AlipayTradeWapPayRequest wapPayRequest;
        private AlipayTradePagePayRequest pagePayRequest;
        // private AlipayTradeOrderSettleRequest settleRequest;
        private bool createByCert = false;
        public OdinAliPayHelper(AliPayCnfOptions aliPayConfig, bool _createByCert = false)
        {
            createByCert = _createByCert;
            PayConfig = AliPayConfigFactory.GetConfig(aliPayConfig);
            if (!createByCert)
                client = new DefaultAopClient(PayConfig.Gatewayurl, PayConfig.AppId, PayConfig.PrivateKey, PayConfig.AliFormat, PayConfig.Version,
                    PayConfig.SignType, PayConfig.AlipayPublicKey, PayConfig.CharSet, false);
            else
                client = new DefaultAopClient(PayConfig.Gatewayurl, PayConfig.AppId, PayConfig.PrivateKey, PayConfig.AliFormat, PayConfig.Version,
                    PayConfig.SignType, PayConfig.CharSet, false, this.GetCertParams(PayConfig));

        }

        private CertParams GetCertParams(AliPayConfig config)
        {
            return new CertParams
            {
                //请更换为您的应用公钥证书文件路径
                AlipayPublicCertPath = config.CertPath.AlipayPublicKeyPath,
                //请更换您的支付宝公钥证书文件路径
                AppCertPath = config.CertPath.AppPublicKeyPath,
                //请更换为支付宝根证书文件路径
                RootCertPath = config.CertPath.AliRootPath,
            };
        }

        /// <summary>
        /// 获取支付宝支付类型对应参数
        /// </summary>
        /// <param name="payType">AliPayTypeEnum枚举 手机app支付，手机网站支付，pc网页支付，面对面支付</param>
        /// <code>
        /// </code>
        /// <returns></returns>
        public string GetPayType(AliPayTypeEnum payType) =>
            new Dictionary<AliPayTypeEnum, string> { { AliPayTypeEnum.AppPay, "QUICK_MSECURITY_PAY" },
                { AliPayTypeEnum.AppWebPay, "QUICK_WAP_WAY" },
                { AliPayTypeEnum.PcWebPay, "FAST_INSTANT_TRADE_PAY" },
                { AliPayTypeEnum.FacePay, "FACE_TO_FACE_PAYMENT" },
            }[payType];

        /// <summary>
        /// 支付请求
        /// </summary>
        /// <param name="payType">支付类型，枚举( 手机app支付，手机网站支付，pc网页支付 )</param>
        /// <param name="payModel">支付数据模型</param>
        /// <returns>返回支付请求响应</returns>
        public AopResponse PayRequest(AliPayTypeEnum payType, AopObject payModel)
        {
            switch (payType)
            {
                /// <summary>
                /// AlipayTradeWapPayRequest
                /// </summary>
                /// <returns></returns>
                case AliPayTypeEnum.AppPay:
                    appPayRequest = new AlipayTradeAppPayRequest();
                    appPayRequest.SetReturnUrl(PayConfig.AliReturnUrl);
                    appPayRequest.SetNotifyUrl(PayConfig.AliNotifyUrl);
                    appPayRequest.SetBizModel(payModel);
                    aopResponse = client.SdkExecute(appPayRequest);
                    break;
                case AliPayTypeEnum.AppWebPay:
                    wapPayRequest = new AlipayTradeWapPayRequest();
                    wapPayRequest.SetReturnUrl(PayConfig.AliReturnUrl);
                    wapPayRequest.SetNotifyUrl(PayConfig.AliNotifyUrl);
                    wapPayRequest.SetBizModel(payModel);
                    aopResponse = client.Execute(wapPayRequest);
                    break;
                case AliPayTypeEnum.PcWebPay:
                    pagePayRequest = new AlipayTradePagePayRequest();
                    pagePayRequest.SetReturnUrl(PayConfig.AliReturnUrl);
                    pagePayRequest.SetNotifyUrl(PayConfig.AliNotifyUrl);
                    pagePayRequest.SetBizModel(payModel);
                    aopResponse = client.Execute(pagePayRequest);
                    break;
                default:
                    break;
            }
            return aopResponse;
        }

        /// <summary>
        /// alipay.fund.trans.uni.transfer(单笔转账接口)
        /// </summary>
        /// <param name="payModel"></param>
        /// <returns></returns>
        public AopResponse AFTUT_Request(AopObject payModel)
        {
            AlipayFundTransUniTransferRequest request = new AlipayFundTransUniTransferRequest();
            request.SetReturnUrl(PayConfig.AliReturnUrl);
            request.SetNotifyUrl(PayConfig.AliNotifyUrl);
            request.SetBizModel(payModel);
            aopResponse = !createByCert ? client.Execute(request) : client.CertificateExecute(request);
            return aopResponse;
        }
    }
}