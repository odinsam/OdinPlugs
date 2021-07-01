using System;
using System.Collections.Generic;

namespace OdinPlugs.OdinCore.Models
{
    public class MailSendModel
    {
        public MailSendModel(bool enableSsl = false)
        {
            SendConfig = new SendMailServerConfig();
            SendConfig.EnableSsl = enableSsl;
        }
        public SendMailServerConfig SendConfig { get; set; }
        public string Subject { get; set; }
        public string MailDateTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        public int Templateid { get; set; }
        public MailFromUserModel FromUser { get; set; }
        public List<MailToUserModel> ToUsers { get; set; }
        public List<MailCcUserModel> CcUsers { get; set; }
        public List<string> Files { get; set; }
        public string Content { get; set; }
    }

    public class MailFromUserModel : MailUserModel
    {
        public string UserPwd { get; set; }
    }
    public class MailToUserModel : MailUserModel
    {

    }
    public class MailCcUserModel : MailUserModel
    {

    }
    public class MailUserModel
    {
        public string UserName { get; set; }
        public string UserAddress { get; set; }
    }
    public class SendMailServerConfig
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; } = 25;
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set; }
        public bool EnableSsl { get; set; }

    }
}