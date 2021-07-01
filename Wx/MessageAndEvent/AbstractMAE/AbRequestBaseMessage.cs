using System;
using System.Threading.Tasks;
using OdinPlugs.OdinCore.ConfigModel.WxParamsModel.Core;
using OdinPlugs.Wx.MessageAndEvent.IMAE;
using OdinPlugs.Wx.Models;
using OdinPlugs.Wx.Models.MAEModel;

namespace OdinPlugs.Wx.MessageAndEvent.AbstractMAE
{
    /// <summary>
    /// 用户发送消息请求
    /// </summary>
    public abstract class AbRequestBaseMessage : AbMAEBase, IMAERequestBaseMessage
    {
        public AbRequestBaseMessage(BaseMessageModel _messageModel, MAEParamsModel _paramsModel, WxCnfOptions _wxOptions)
                                    : base(_messageModel, _paramsModel, _wxOptions)
        {
        }

        public abstract string RequestImageMessage(Action<object> callBack);
        public abstract string RequestLinkMessage(Action<object> callBack);
        public abstract string RequestLocationMessage(Action<object> callBack);
        public abstract string RequestShortVideoMessage(Action<object> callBack);
        public abstract string RequestTextMessage(Action<object> callBack);
        public abstract string RequestVideoMessage(Action<object> callBack);
        public abstract string RequestVoiceMessage(Action<object> callBack);
    }
}