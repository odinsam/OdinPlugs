namespace OdinPlugs.Wx.Models.Pay
{
    public class JsPayModel
    {
        private readonly WxConfig wxConfig;
        public JsPayModel()
        {
            wxConfig = new WxConfig();
        }
        public string appId
        {
            get
            {
                return wxConfig.GetAppID();
            }
        }

        public string timeStamp { get; set; }

        public string nonceStr { get; set; }

        /// <summary>
        /// 订单详情扩展字符串   e.g 统一下单接口返回的prepay_id参数值
        /// </summary>
        /// <value></value>
        public string package { get; set; }

        public string signType { get; set; } = "MD5";

        public string paySign { get; set; }

    }
}