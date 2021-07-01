namespace OdinPlugs.Wx.Models.Pay
{
    /// <summary>
    /// wx交易商品信息模型    e.g 具体请见 https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=4_2
    /// </summary>
    public class Product
    {
        /// <summary>
        /// 商品描述   e.g 商品简单描述，该字段请按照规范传递，
        /// </summary>
        /// <value></value>
        public string body { get; set; }

        /// <summary>
        /// 商品详情  e.g 商品详细描述，对于使用单品优惠的商户，该字段必须按照规范上传
        /// </summary>
        /// <value></value>
        public string detail { get; set; } = null;
        /// <summary>
        /// 附加数据   e.g 附加数据，在查询API和支付通知中原样返回，可作为自定义参数使用。
        /// </summary>
        /// <value></value>
        public string attach { get; set; } = null;

        /// <summary>
        /// 订单号   e.g 商户系统内部订单号，要求32个字符内，只能是数字、大小写字母_-|* 且在同一个商户号下唯一。
        /// </summary>
        /// <value></value>
        public string out_trade_no { get; set; }

        /// <summary> 
        /// 标价币种    e.g 符合ISO 4217标准的三位字母代码，默认人民币：CNY
        /// </summary>
        /// <value></value>
        public string fee_type { get; set; } = "CNY";

        /// <summary>
        /// 标价金额   e.g 订单总金额，单位为分
        /// </summary>
        /// <value></value>
        public int total_fee { get; set; }

        /// <summary>
        /// 交易起始时间
        /// </summary>
        /// <value></value>
        public string time_start { get; set; } = null;

        /// <summary>
        /// 交易结束时间s
        /// </summary>
        /// <value></value>
        public string time_expire { get; set; } = null;

        /// <summary>
        /// 订单优惠标记   e.g 订单优惠标记，使用代金券或立减优惠功能时需要的参数
        /// </summary>
        /// <value></value>
        public string goods_tag { get; set; } = null;

        /// <summary>
        /// 商品定义  e.g trade_type=NATIVE时，此参数必传。此参数为二维码中包含的商品ID，商户自行定义。
        /// </summary>
        /// <value></value>
        public string product_id { get; set; } = null;

    }
}