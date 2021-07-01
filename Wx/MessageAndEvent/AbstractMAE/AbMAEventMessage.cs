using System;
using System.Threading.Tasks;
using OdinPlugs.OdinCore.ConfigModel.WxParamsModel.Core;
using OdinPlugs.Wx.MessageAndEvent.IMAE;
using OdinPlugs.Wx.Models;
using OdinPlugs.Wx.Models.MAEModel;

namespace OdinPlugs.Wx.MessageAndEvent.AbstractMAE
{
    public abstract class AbMAEEventMessage : AbMAEBase, IMAEEventMessage
    {

        public AbMAEEventMessage(BaseMessageModel _messageModel, MAEParamsModel _paramsModel, WxCnfOptions _wxOptions)
                                    : base(_messageModel, _paramsModel, _wxOptions)
        {
        }

        public abstract string RequestEventClickMessage(Action<object> callBack);
        public abstract string RequestEventLocationSelectMessage(Action<object> callBack);
        public abstract string RequestEventMassSendJonFinishMessage(Action<object> callBack);
        public abstract string RequestEventPicMessage(Action<object> callBack);
        public abstract string RequestEventReportLocationMessage(Action<object> callBack);
        public abstract string RequestEventScanCodeMessage(Action<object> callBack);
        public abstract string RequestEventScanMessage(Action<object> callBack);
        public abstract string RequestEventSubscribeMessage(Action<object> callBack);
        public abstract string RequestEventTemplateSendJobFinishMessage(Action<object> callBack);
        public abstract string RequestEventUnSubscribeMessage(Action<object> callBack);
        public abstract string RequestEventViewMessage(Action<object> callBack);
    }
}