namespace OdinPlugs.Wx.Models.MAEModel.MessageModel
{
    /// <summary>
    /// 图像消息
    /// </summary>
    public class ImageMessageModel : NormalMessageModel
    {
        /// <summary>
        /// 图片路径
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 媒体ID
        /// </summary>
        public string MediaId { get; set; }
    }
}