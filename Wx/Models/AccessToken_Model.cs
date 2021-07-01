namespace OdinPlugs.Wx.Models
{
    public class AccessToken_Model
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public long createTime { get; set; }
        public string refresh_token { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }

    }
}