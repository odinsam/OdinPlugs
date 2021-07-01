using System.Security.Cryptography.X509Certificates;
namespace OdinPlugs.Wx.Models
{
    public class MessageOptions
    {
        public string Template { get; set; }
        public string DefaultMessage { get; set; }
    }

    /// <summary>
    /// 消息类配置
    /// </summary>
    public class MessagesOptions
    {
        /// <summary>
        /// 欢迎语
        /// </summary>
        /// <value></value>
        public MessageOptions Greeting { get; set; }
        public MessageOptions NormalTextMessage { get; set; }
    }

    public class ApiInfo
    {
        public string ApiPath { get; set; }
        public string Method { get; set; }
    }

    public class MenuApiOption
    {
        public ApiInfo CreateMenu { get; set; }
    }

    public class MaterialApiOption
    {
        public ApiInfo UploadImage { get; set; }
    }

    public class UserApiApiOption
    {
        public ApiInfo NoFollowGetUserInfo { get; set; }
        public ApiInfo GetAccess_TokenByCode { get; set; }
        public ApiInfo RefreshToken { get; set; }
        public ApiInfo FollowGetUserInfo { get; set; }
        public ApiInfo CheckAccess_Token { get; set; }
        public ApiInfo GetUserList { get; set; }
        public ApiInfo GetUserInfo { get; set; }
    }

    public class ApiOptions
    {
        public string Url { get; set; }
        /// <summary>
        /// 菜单相关接口
        /// </summary>
        /// <value></value>
        public MenuApiOption MenuApis { get; set; }
        public UserApiApiOption UserApis { get; set; }
        public MaterialApiOption Materials { get; set; }
    }
    public class JsPageAuthenCallBackOptions
    {
        /// <summary>
        /// js回调url
        /// </summary>
        /// <value></value>
        public string Url { get; set; }
        /// <summary>
        /// js页面授权完成后的默认跳转页面
        /// </summary>
        /// <value></value>
        public string DefaultPage { get; set; }
    }
    public class PayApisOptions
    {
        public ApiInfo UnifiedOrder { get; set; }
    }
    public class PayNotifyApisOptions
    {
        public ApiInfo Notify { get; set; }
    }
    public class PayNotifyOptions
    {
        public string Url { get; set; }
        public PayNotifyApisOptions Apis { get; set; }
    }
    public class PayOptions
    {
        public string Url { get; set; }
        public PayApisOptions Apis { get; set; }
    }

    /// <summary>
    /// wx配置文件实体类
    /// </summary>
    public class WxCnfOptions
    {
        /// <summary>
        /// js回调地址
        /// </summary>
        /// <value></value>
        public JsPageAuthenCallBackOptions JsPageAuthenCallBack { get; set; }
        /// <summary>
        /// 微信支付接口配置
        /// </summary>
        /// <value></value>
        public PayOptions Pay { get; set; }
        public PayNotifyOptions PayNotify { get; set; }
        /// <summary>
        /// 接口配置
        /// </summary>
        /// <value></value>
        public ApiOptions Api { get; set; }
        /// <summary>
        /// 消息类配置
        /// </summary>
        public MessagesOptions Messages { get; set; }
    }
}