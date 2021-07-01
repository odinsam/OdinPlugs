using System;
using Newtonsoft.Json.Linq;
using OdinPlugs.Wx.MessageAndEvent.AbstractMAE;
using OdinPlugs.Wx.MessageAndEvent.MAEHandler;
using OdinPlugs.Wx.Models.MAEModel;
using OdinPlugs.Wx.Models.MAEModel.MessageModel;

namespace OdinPlugs.Wx.MessageAndEvent
{
    public class MAEBaseFactory
    {
        public static IMAEMessageHandler MAEMessageHandler(AbMAEBase eventMessageHandler)
        {
            MsgEnumType type = eventMessageHandler.MessageModel.MsgType;
            switch (type)
            {
                case MsgEnumType.EVENT:
                    return new EventMessageHandler();
                default:
                    return new RequestBaseMessageHandler();
            }
        }
    }
}