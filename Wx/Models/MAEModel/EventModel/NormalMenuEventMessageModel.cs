namespace OdinPlugs.Wx.Models.MAEModel.EventModel
{
    /// <summary>
    /// 普通菜单事件，包括click和view
    /// </summary>
    public class NormalMenuEventMessageModel : EventMessageModel
    {
        /// <summary>
        /// 事件KEY值，设置的跳转URL
        /// </summary>
        public string EventKey { get; set; }
    }
}