using System;
using System.Threading.Tasks;

namespace OdinPlugs.Wx.MessageAndEvent.IMAE
{
    /// <summary>
    /// 响应消息处理接口
    /// </summary>
    public interface IMAEResponseBaseMessage : IMAEBase
    {
        /// <summary>
        /// 文本消息
        /// </summary>
        string ResponseTextMessage(Action<Object> callBack);
        /// <summary>
        /// 图片消息
        /// </summary>
        string ResponseImageMessage(Action<Object> callBack);
        /// <summary>
        /// 语音消息
        /// </summary>
        string ResponseVoiceMessage(Action<Object> callBack);
        /// <summary>
        /// 视频消息
        /// </summary>
        string ResponseVideoMessage(Action<Object> callBack);
        /// <summary>
        /// 音乐消息
        /// </summary>
        string ResponseMusicMessage(Action<Object> callBack);
        /// <summary>
        /// 图文消息
        /// </summary>
        string ResponseNewsMessage(Action<Object> callBack);
        /// <summary>
        /// 转发客服消息
        /// </summary>
        string ResponseTransferCustomerServiceMessage(Action<Object> callBack);
    }
}