using System;
using System.Threading.Tasks;
using OdinPlugs.OdinCore.ConfigModel.WxParamsModel.Core;
using OdinPlugs.Wx.MessageAndEvent.IMAE;
using OdinPlugs.Wx.Models;
using OdinPlugs.Wx.Models.MAEModel;

namespace OdinPlugs.Wx.MessageAndEvent.AbstractMAE
{
    public abstract class AbResponseBaseMessage : AbMAEBase, IMAEResponseBaseMessage
    {
        public AbResponseBaseMessage(BaseMessageModel _messageModel, MAEParamsModel _paramsModel, WxCnfOptions _wxOptions)
                                    : base(_messageModel, _paramsModel, _wxOptions)
        {
        }

        public abstract string ResponseImageMessage(Action<object> callBack);
        public abstract string ResponseMusicMessage(Action<object> callBack);
        public abstract string ResponseNewsMessage(Action<object> callBack);
        public abstract string ResponseTextMessage(Action<object> callBack);
        public abstract string ResponseTransferCustomerServiceMessage(Action<object> callBack);
        public abstract string ResponseVideoMessage(Action<object> callBack);
        public abstract string ResponseVoiceMessage(Action<object> callBack);
    }
}