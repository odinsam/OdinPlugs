using OdinPlugs.Wx.Models.MAEModel.EventModel;
using OdinPlugs.Wx.Models.MAEModel.MessageModel;

namespace OdinPlugs.Wx.Models.MAEModel
{
    public abstract class BaseMessageModel
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 发送方帐号（一个OpenID）
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public MsgEnumType MsgType { get; set; }
        public EventEnumType Event { get; set; }

        public string OpenId { get; set; }
        public string MsgId { get; set; }
    }
}