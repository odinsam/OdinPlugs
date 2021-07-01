using System;
using OdinPlugs.Wx.MessageAndEvent.AbstractMAE;
using OdinPlugs.Wx.Models.MAEModel;
using OdinPlugs.Wx.Models.MAEModel.EventModel;
using System.Threading.Tasks;
namespace OdinPlugs.Wx.MessageAndEvent.MAEHandler
{
    public class EventMessageHandler : IMAEMessageHandler
    {
        public EventMessageHandler()
        {
        }
        public string Handler(AbMAEBase messageHandler, Action<Object> callBack)
        {
            AbMAEEventMessage eventMessageHandler = messageHandler as AbMAEEventMessage;
            BaseMessageModel messageModel = eventMessageHandler.MessageModel;
            switch (messageModel.Event)
            {
                case EventEnumType.CLICK: return eventMessageHandler.RequestEventClickMessage(callBack);
                case EventEnumType.VIEW: return eventMessageHandler.RequestEventViewMessage(callBack);
                case EventEnumType.LOCATION: return eventMessageHandler.RequestEventReportLocationMessage(callBack);
                case EventEnumType.LOCATION_SELECT: return eventMessageHandler.RequestEventLocationSelectMessage(callBack);
                case EventEnumType.SCAN: return eventMessageHandler.RequestEventScanMessage(callBack);
                case EventEnumType.SUBSCRIBE: return eventMessageHandler.RequestEventSubscribeMessage(callBack);
                case EventEnumType.UNSUBSCRIBE: return eventMessageHandler.RequestEventUnSubscribeMessage(callBack);
                case EventEnumType.SCANCODE_WAITMSG: return eventMessageHandler.RequestEventScanCodeMessage(callBack);
            }
            return null;
        }
    }
}