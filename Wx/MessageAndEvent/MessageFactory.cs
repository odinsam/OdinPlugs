using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using OdinPlugs.OdinUtils.Utils.OdinTime;
using OdinPlugs.Wx.Models.MAEModel;
using OdinPlugs.Wx.Models.MAEModel.EventModel;
using OdinPlugs.Wx.Models.MAEModel.MessageModel;

namespace OdinPlugs.Wx.MessageAndEvent
{
    public class MAEFactory
    {
        private static Dictionary<string, BaseMsg> msgDic = WxMessageDic.GetWxMessageQueue();
        public static BaseMessageModel CreateMessage(string xml)
        {
            //消息队列超过50个 将消息出队，只保留20s以内未处理的消息
            if (msgDic.Count >= 50)
            {
                foreach (var key in msgDic.Keys)
                {
                    var item = msgDic[key];
                    if (item.CreateTime + 20 > UnixTimeHelper.GetUnixDateTime())
                        msgDic.Remove(key);
                }
            }
            XElement xdoc = XElement.Parse(xml);
            var msgtype = xdoc.Element("MsgType").Value.ToUpper();
            var FromUserName = xdoc.Element("FromUserName").Value;
            var msgId = String.Empty;
            if (xdoc.Elements().Where(x => x.Value == "MsgId").SingleOrDefault() != null)
                msgId = xdoc.Element("MsgId").Value;
            var CreateTime = Convert.ToInt64(xdoc.Element("CreateTime").Value);
            BaseMsg baseMsg = new BaseMsg { FromUser = FromUserName, CreateTime = CreateTime, MsgId = msgId };
            //消息排重
            //首先判断msgid是否存在 如果不存在 消息入队
            if (!msgDic.ContainsKey(msgId) && !msgDic.ContainsKey(FromUserName + CreateTime))
            {
                var key = string.IsNullOrEmpty(msgId) ? FromUserName + CreateTime : msgId;
                msgDic.Add(key, baseMsg);
                System.Console.WriteLine("处理消息入队");
                System.Console.WriteLine($"MsgId:{msgId}\r\nFromUserName:{FromUserName}\r\nCreateTime:{CreateTime}");
            }
            MsgEnumType type = (MsgEnumType)Enum.Parse(typeof(MsgEnumType), msgtype);
            System.Console.WriteLine($"待转换xml内容:\r\n{xml}");
            switch (type)
            {
                case MsgEnumType.TEXT: return WxTools.ConvertObj<TextMessageModel>(xml);
                case MsgEnumType.IMAGE: return WxTools.ConvertObj<ImageMessageModel>(xml);
                case MsgEnumType.VIDEO: return WxTools.ConvertObj<VideoMessageModel>(xml);
                case MsgEnumType.VOICE: return WxTools.ConvertObj<VoiceMessageModel>(xml);
                case MsgEnumType.LINK:
                    return WxTools.ConvertObj<LinkMessageModel>(xml);
                case MsgEnumType.LOCATION:
                    return WxTools.ConvertObj<LocationMessageModel>(xml);
                case MsgEnumType.EVENT://事件类型
                    {
                        var eventtype = (EventEnumType)Enum.Parse(typeof(EventEnumType), xdoc.Element("Event").Value.ToUpper());
                        switch (eventtype)
                        {
                            case EventEnumType.CLICK:
                                return WxTools.ConvertObj<NormalMenuEventMessageModel>(xml);
                            case EventEnumType.VIEW: return WxTools.ConvertObj<NormalMenuEventMessageModel>(xml);
                            case EventEnumType.LOCATION: return WxTools.ConvertObj<LocationEventMessageModel>(xml);
                            // case EventEnumType.LOCATION_SELECT: return Utils.ConvertObj<LocationMenuEventMessageModel>(xml);
                            case EventEnumType.SCAN: return WxTools.ConvertObj<ScanEventMessageModel>(xml);
                            case EventEnumType.SUBSCRIBE: return WxTools.ConvertObj<SubEventMessageModel>(xml);
                            case EventEnumType.UNSUBSCRIBE: return WxTools.ConvertObj<SubEventMessageModel>(xml);
                            case EventEnumType.SCANCODE_WAITMSG: return WxTools.ConvertObj<ScanMenuEventMessageModel>(xml);
                            default:
                                System.Console.WriteLine(eventtype.ToString());
                                return WxTools.ConvertObj<BaseMessageModel>(xml);
                        }
                    }
                default:
                    System.Console.WriteLine(type.ToString());
                    return WxTools.ConvertObj<BaseMessageModel>(xml);
            }
        }

        public static void RemoveBaseMsgDic(BaseMessageModel messageModel)
        {
            if (string.IsNullOrEmpty(messageModel.MsgId))
                msgDic.Remove(messageModel.FromUserName + messageModel.CreateTime);
            else
                msgDic.Remove(messageModel.MsgId);
            System.Console.WriteLine($"处理消息出队 \r\nMsgId:{messageModel.MsgId}\r\nFromUserName:{messageModel.FromUserName}\r\nCreateTime:{messageModel.CreateTime}");
        }
    }
}