using System;
using System.Threading.Tasks;

namespace OdinPlugs.Wx.MessageAndEvent.IMAE
{
    /// <summary>
    /// 请求消息处理接口
    /// </summary>
    public interface IMAERequestBaseMessage : IMAEBase
    {
        /// <summary>
        /// 文本消息
        /// </summary>
        string RequestTextMessage(Action<Object> callBack);
        /// <summary>
        /// 文本消息
        /// </summary>
        string RequestImageMessage(Action<Object> callBack);
        /// <summary>
        /// 语音消息
        /// </summary>
        string RequestVoiceMessage(Action<Object> callBack);
        /// <summary>
        /// 视频消息
        /// </summary>
        string RequestVideoMessage(Action<Object> callBack);
        /// <summary>
        /// 视频消息
        /// </summary>
        string RequestShortVideoMessage(Action<Object> callBack);
        /// <summary>
        /// 地理位置消息
        /// </summary>
        string RequestLocationMessage(Action<Object> callBack);
        /// <summary>
        /// 链接消息
        /// </summary>
        string RequestLinkMessage(Action<Object> callBack);
    }
}