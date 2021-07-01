namespace OdinPlugs.Wx.Models.MAEModel.EventModel
{
    /// <summary>
    /// 上报地理位置实体
    /// </summary>
    public class LocationEventMessageModel : EventMessageModel
    {
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 地理位置精度
        /// </summary>
        public string Precision { get; set; }
    }
}