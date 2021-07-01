using Newtonsoft.Json.Linq;
using OdinPlugs.OdinCore.ConfigModel.WxParamsModel.Core;
using OdinPlugs.Wx.MessageAndEvent.IMAE;
using OdinPlugs.Wx.Models;
using OdinPlugs.Wx.Models.MAEModel;
using OdinPlugs.Wx.Models.MAEModel.EventModel;
using OdinPlugs.Wx.Models.MAEModel.MessageModel;

namespace OdinPlugs.Wx.MessageAndEvent.AbstractMAE
{
    public abstract class AbMAEBase : IMAEBase
    {
        public MAEParamsModel ParamsModel { get; set; }
        public WxCnfOptions WxOptions { get; set; }
        public MsgEnumType MsgType { get; set; }
        public EventEnumType Event { get; set; }
        public BaseMessageModel MessageModel { get; set; }
        public AbMAEBase(BaseMessageModel _messageModel, MAEParamsModel _paramsModel, WxCnfOptions _wxOptions)
        {
            MessageModel = _messageModel;
            ParamsModel = _paramsModel;
            WxOptions = _wxOptions;
            MsgType = _messageModel.MsgType;
            Event = _messageModel.Event;
        }
    }
}