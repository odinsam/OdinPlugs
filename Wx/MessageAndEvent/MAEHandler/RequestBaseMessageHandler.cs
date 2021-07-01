using System;
using System.Threading.Tasks;
using OdinPlugs.Wx.MessageAndEvent.AbstractMAE;
using OdinPlugs.Wx.Models.MAEModel;
using OdinPlugs.Wx.Models.MAEModel.MessageModel;

namespace OdinPlugs.Wx.MessageAndEvent.MAEHandler
{
    public class RequestBaseMessageHandler : IMAEMessageHandler
    {
        public RequestBaseMessageHandler()
        {
        }

        string IMAEMessageHandler.Handler(AbMAEBase messageHandler, Action<Object> callBack)
        {
            AbRequestBaseMessage eventMessageHandler = messageHandler as AbRequestBaseMessage;
            BaseMessageModel messageModel = eventMessageHandler.MessageModel;
            switch (messageModel.MsgType)
            {
                case MsgEnumType.TEXT: return eventMessageHandler.RequestTextMessage(callBack);
                case MsgEnumType.IMAGE: return eventMessageHandler.RequestImageMessage(callBack);
                case MsgEnumType.VOICE: return eventMessageHandler.RequestVoiceMessage(callBack);
                case MsgEnumType.VIDEO: return eventMessageHandler.RequestVideoMessage(callBack);
                case MsgEnumType.LOCATION: return eventMessageHandler.RequestLocationMessage(callBack);
                case MsgEnumType.LINK: return eventMessageHandler.RequestLinkMessage(callBack);
                case MsgEnumType.SHORTVideo: return eventMessageHandler.RequestShortVideoMessage(callBack);
            }
            return null;
        }
    }
}