namespace OdinPlugs.Wx.Models.MAEModel.MessageModel
{
    /// <summary>
    /// 语音消息
    /// </summary>
    public class VoiceMessageModel : NormalMessageModel
    {
        /// <summary>
        /// 格式
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// 媒体ID
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 语音识别结果
        /// </summary>
        public string Recognition { get; set; }
    }
}