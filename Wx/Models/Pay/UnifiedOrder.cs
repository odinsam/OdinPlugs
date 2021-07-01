namespace OdinPlugs.Wx.Models.Pay
{
    public class UnifiedOrder : Product
    {
        private readonly WxConfig wxConfig;
        public UnifiedOrder()
        {
            wxConfig = new WxConfig();
        }
        /// <summary>
        /// 公众账号ID  e.g 微信支付分配的公众账号ID（企业号corpid即为此appId）
        /// </summary>
        /// <value></value>
        public string appid
        {
            get
            {
                return wxConfig.GetAppID();
            }
        }

        /// <summary>
        /// 商户号   e.g 微信支付分配的商户号
        /// </summary>
        /// <value></value>
        public string mch_id
        {
            get
            {
                return wxConfig.GetMchID();
            }
        }

        /// <summary>
        /// 设备号	 e.g 自定义参数，可以为终端设备号(门店号或收银设备ID)，PC网页或公众号内支付可以传"WEB"
        /// </summary>
        /// <value></value>
        public string device_info { get; set; } = "WEB";

        /// <summary>
        /// 随机字符串 e.g 随机字符串，长度要求在32位以内。
        /// </summary>
        /// <value></value>
        public string nonce_str
        {
            get
            {
                return WxPayApi.GenerateNonceStr();
            }
        }

        /// <summary>
        /// 签名  e.g 通过签名算法计算得出的签名值    详见 https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=4_3
        /// </summary>
        /// <value></value>
        public string sign { get; set; }

        /// <summary>
        /// 签名类型   e.g 签名类型，默认为MD5，支持HMAC-SHA256和MD5。
        /// </summary> 
        /// <value></value>
        public string sign_type { get; set; } = ((SignTypeEnum)0).ToString().Replace("_", "-");

        /// <summary>
        /// 终端IP   e.g  支持IPV4和IPV6两种格式的IP地址。用户的客户端IP
        /// </summary>
        /// <value></value>
        public string spbill_create_ip { get; set; }

        /// <summary>
        /// 异步接收微信支付结果通知的回调地址，通知url必须为外网可访问的url，不能携带参数。
        /// </summary>
        /// <value></value>
        public string notify_url
        {
            get
            {
                return wxConfig.GetNotifyUrl();
            }
        }

        /// <summary>
        /// 交易类型  e.g TradeType enum
        /// </summary>
        /// <value></value>
        public string trade_type { get; set; }

        /// <summary>
        /// 指定支付方式      e.g 上传此参数no_credit--可限制用户不能使用信用卡支付
        /// </summary>
        /// <value></value>
        public string limit_pay { get; set; } = null;

        /// <summary>
        /// 用户标识   e.g trade_type=JSAPI时（即JSAPI支付），此参数必传，此参数为微信用户在商户对应appid下的唯一标识
        /// </summary>
        /// <value></value>
        public string openid { get; set; }

        /// <summary>
        /// 电子发票入口开放标识     e.g  Y，传入Y时，支付成功消息和支付详情页将出现开票入口。需要在微信支付商户平台或微信公众平台开通电子发票功能，传此字段才可生效
        /// </summary>
        /// <value></value>
        public string receipt { get; set; } = null;

        // private string sceneinfo;

        /// <summary>
        /// 场景信息   e.g 该字段常用于线下活动时的场景信息上报，支持上报实际门店信息，商户也可以按需求自己上报相关信息。该字段为Scene对象的JSON数据，对象格式为{"store_info":{"id": "门店ID","name": "名称","area_code": "编码","address": "地址" }} ，字段详细说明参见Scene类
        /// </summary>
        /// <value></value>
        public string scene_info { get; set; } = null;
    }

    public class Scene
    {
        /// <summary>
        /// 门店编号 e.g 由商户自定义
        /// </summary>
        /// <value></value>
        public string id { get; set; }

        /// <summary>
        /// 门店名称 e.g 门店名称 ，由商户自定义
        /// </summary>
        /// <value></value>
        public string name { get; set; }

        /// <summary>
        /// 门店行政区划码 e.g 门店所在地行政区划码，详细见《最新县及县以上行政区划代码》https://pay.weixin.qq.com/wiki/doc/api/download/store_adress.csv
        /// </summary>
        /// <value></value>
        public string area_code { get; set; }

        /// <summary>
        /// 门店详细地址 e.g 门店详细地址 ，由商户自定义
        /// </summary>
        /// <value></value>
        public string address { get; set; }
    }

    public enum TradeTypeEnum
    {
        JSAPI,   //JSAPI支付
        NATIVE, //Native支付
        APP //APP支付
    }

    public enum SignTypeEnum
    {
        MD5,
        HMAC_SHA256
    }

}