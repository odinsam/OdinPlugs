namespace OdinPlugs.Wx.Models.MAEModel.MessageModel
{
    public class LocationMessageModel : NormalMessageModel
    {

        /// <summary>
        /// X坐标
        /// </summary>
        /// <value></value>
        public float Location_X { get; set; }
        /// <summary>
        /// Y 坐标
        /// </summary>
        /// <value></value>
        public float Location_Y { get; set; }
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        /// <value></value>
        public int Scale { get; set; }
        /// <summary>
        /// 地理位置信息
        /// </summary>
        /// <value></value>
        public string Label { get; set; }
    }
}