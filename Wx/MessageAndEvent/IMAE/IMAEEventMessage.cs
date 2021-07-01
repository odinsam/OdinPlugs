using System;
using System.Threading.Tasks;
namespace OdinPlugs.Wx.MessageAndEvent.IMAE
{
    /// <summary>
    /// 请求推送事件
    /// </summary>
    public interface IMAEEventMessage : IMAEBase
    {
        /// <summary>
        /// 订阅事件
        /// </summary>
        string RequestEventSubscribeMessage(Action<Object> callBack);
        /// <summary>
        /// 取消订阅事件
        /// </summary>
        string RequestEventUnSubscribeMessage(Action<Object> callBack);
        /// <summary>
        /// 扫描二维码事件
        /// </summary>
        string RequestEventScanMessage(Action<Object> callBack);
        /// <summary>
        /// 上报地理位置事件
        /// </summary>
        string RequestEventReportLocationMessage(Action<Object> callBack);
        /// <summary>
        /// 点击菜单拉去消息事件
        /// </summary>
        string RequestEventClickMessage(Action<Object> callBack);
        /// <summary>
        /// 点击菜单跳转链接事件
        /// </summary>
        string RequestEventViewMessage(Action<Object> callBack);
        /// <summary>
        /// 点击菜单扫码事件
        /// </summary>
        string RequestEventScanCodeMessage(Action<Object> callBack);
        /// <summary>
        /// 点击菜单发送图片事件
        /// </summary>
        string RequestEventPicMessage(Action<Object> callBack);
        /// <summary>
        /// 点击菜单选择地理位置事件
        /// </summary>
        string RequestEventLocationSelectMessage(Action<Object> callBack);
        /// <summary>
        /// 推送群发消息结果事件
        /// </summary>
        string RequestEventMassSendJonFinishMessage(Action<Object> callBack);
        /// <summary>
        /// 推送发送模板消息结果事件
        /// </summary>
        string RequestEventTemplateSendJobFinishMessage(Action<Object> callBack);

    }
}