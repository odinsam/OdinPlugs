using System;

namespace OdinPlugs.Wx.Models.MAEModel
{
    public class BaseMsg
    {
        public string FromUser { get; set; } = string.Empty;
        /// <summary>
        /// 消息表示。普通消息时，为msgid，事件消息时，为事件的创建时间
        /// </summary>
        public string MsgId { get; set; } = string.Empty;
        /// <summary>
        /// 添加到队列的时间
        /// </summary>
        public long CreateTime { get; set; }
    }
}