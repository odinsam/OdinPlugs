namespace OdinPlugs.Wx.Models.Pay
{
    /// <summary>
    /// 回调通知模型
    /// </summary>
    public class PayNotifyModel
    {
        /// <summary>
        /// 返回状态码
        /// </summary>
        /// <value></value>
        public string return_code { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        /// <value></value>
        public string return_msg { get; set; }

        //以下字段在return_code为SUCCESS的时候有返回

        /// <summary>
        /// 公众账号ID   e.g 微信分配的公众账号ID（企业号corpid即为此appId）
        /// </summary>
        /// <value></value>
        public string appid { get; set; }

        /// <summary>
        /// 商户号  e.g 微信支付分配的商户号
        /// </summary>
        /// <value></value>
        public string mch_id { get; set; }

        /// <summary>
        /// 设备号  e.g 微信支付分配的终端设备号
        /// </summary>
        /// <value></value>
        public string device_info { get; set; } = null;

        /// <summary>
        /// 随机字符串  e.g 	随机字符串，不长于32位
        /// </summary>
        /// <value></value>
        public string nonce_str { get; set; }

        /// <summary>
        /// 签名    e.g 签名，详见签名算法
        /// </summary>
        /// <value></value>
        public string sign { get; set; }

        /// <summary>
        /// 签名类型    e.g 签名类型，目前支持HMAC-SHA256和MD5，默认为MD5
        /// </summary>
        /// <value></value>
        public string sign_type { get; set; } = "MD5";

        /// <summary>
        /// 业务结果    e.g SUCCESS/FAIL
        /// </summary>
        /// <value></value>
        public string result_code { get; set; }

        /// <summary>
        /// 错误代码    e.g 错误返回的错误代码
        /// </summary>
        /// <value></value>
        public string err_code { get; set; } = null;

        /// <summary>
        /// 错误代码描述    e.g 错误返回的信息描述
        /// </summary>
        /// <value></value>
        public string err_code_des { get; set; } = null;

        /// <summary>
        /// 用户标识    e.g 用户在商户appid下的唯一标识
        /// </summary>
        /// <value></value>
        public string openid { get; set; }

        /// <summary>
        /// 是否关注公众账号    e.g 用户是否关注公众账号，Y-关注，N-未关注
        /// </summary>
        /// <value></value>
        public string is_subscribe { get; set; }

        /// <summary>
        /// 交易类型    e.g 	JSAPI、NATIVE、APP
        /// </summary>
        /// <value></value>
        public string trade_type { get; set; }

        /// <summary>
        /// 付款银行    e.g 银行类型，采用字符串类型的银行标识，银行类型见银行列表  https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=4_2
        /// </summary>
        /// <value></value>
        public string bank_type { get; set; }

        /// <summary>
        /// 订单金额    e.g 订单总金额，单位为分
        /// </summary>
        /// <value></value>
        public int total_fee { get; set; }

        /// <summary>
        /// 应结订单金额    e.g 应结订单金额=订单金额-非充值代金券金额，应结订单金额<=订单金额。
        /// </summary>
        /// <value></value>
        public int? settlement_total_fee { get; set; } = null;

        /// <summary>
        /// 货币种类    e.g 货币类型，符合ISO4217标准的三位字母代码，默认人民币：CNY
        /// </summary>
        /// <value></value>
        public string fee_type { get; set; } = "CNY";

        /// <summary>
        /// 现金支付金额    e.g 现金支付金额订单现金支付金额
        /// </summary>
        /// <value></value>
        public int cash_fee { get; set; }

        /// <summary>
        /// 现金支付货币类型    e.g 货币类型，符合ISO4217标准的三位字母代码，默认人民币：CNY
        /// </summary>
        /// <value></value>
        public string cash_fee_type { get; set; } = "CNY";

        /// <summary>
        /// 总代金券金额    e.g 代金券金额<=订单金额，订单金额-代金券金额=现金支付金额
        /// </summary>
        /// <value></value>
        public int? coupon_fee { get; set; } = null;

        /// <summary>
        /// 代金券使用数量  e.g 代金券使用数量
        /// </summary>
        /// <value></value>
        public int? coupon_count { get; set; } = null;

        /// <summary>
        /// 代金券类型  e.g CASH--充值代金券    NO_CASH---非充值代金券并且订单使用了免充值券后有返回（取值：CASH、NO_CASH）。$n为下标,从0开始编号，举例：coupon_type_0
        /// </summary>
        /// <value></value>
        public string coupon_type_n { get; set; } = null;

        /// <summary>
        /// 代金券ID    e.g 代金券ID,$n为下标，从0开始编号
        /// </summary>
        /// <value></value>
        public string coupon_id_n { get; set; }

        /// <summary>
        /// 单个代金券支付金额  e.g 单个代金券支付金额,$n为下标，从0开始编号
        /// </summary>
        /// <value></value>
        public int? coupon_fee_n { get; set; } = null;

        /// <summary>
        /// 微信支付订单号  e.g 微信支付订单号
        /// </summary>
        /// <value></value>
        public string transaction_id { get; set; }

        /// <summary>
        /// 商户订单号  e.g 商户系统内部订单号，要求32个字符内，只能是数字、大小写字母_-|*@ ，且在同一个商户号下唯一。
        /// </summary>
        /// <value></value>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 商家数据包  e.g 商家数据包，原样返回
        /// </summary>
        /// <value></value>
        public string attach { get; set; } = null;

        /// <summary>
        /// 支付完成时间    e.g 支付完成时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010
        /// </summary>
        /// <value></value>
        public string time_end { get; set; }

    }
}