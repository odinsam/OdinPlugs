namespace OdinPlugs.Wx.Models.MAEModel.EventModel
{
    /// <summary>
    /// 订阅/取消订阅事件
    /// </summary>
    public class SubEventMessageModel : EventMessageModel
    {
        private string _eventkey;
        /// <summary>
        /// 事件KEY值，qrscene_为前缀，后面为二维码的参数值（已去掉前缀，可以直接使用）
        /// </summary>
        public string EventKey
        {
            get { return _eventkey; }
            set { _eventkey = value.Replace("qrscene_", ""); }
        }
        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }
    }
}