namespace OdinPlugs.Wx.Models.WxResultModels
{
    public class WxGetFollowUserListResultModel_object
    {
        public string[] openid { get; set; }
    }
    public partial class WxGetFollowUserListResultModel : WxResultModel
    {
        public int total { get; set; }
        public int count { get; set; }
        public WxGetFollowUserListResultModel_object data { get; set; }
        public string next_openid { get; set; }
    }
}