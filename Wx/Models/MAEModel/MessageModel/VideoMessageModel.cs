namespace OdinPlugs.Wx.Models.MAEModel.MessageModel
{
    /// <summary>
    /// 视频消息
    /// </summary>
    public class VideoMessageModel : NormalMessageModel
    {
        /// <summary>
        /// 缩略图ID
        /// </summary>
        public string ThumbMediaId { get; set; }
        /// <summary>
        /// 媒体ID
        /// </summary>
        public string MediaId { get; set; }
    }
}