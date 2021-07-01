using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using OdinPlugs.OdinCore.Models;
using OdinPlugs.OdinUtils.OdinHttp;
using Serilog;

namespace OdinPlugs.OdinUtils.OdinMail
{
    public class MailHelper
    {
        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="smtpserver">SMTP服务器</param>
        /// <param name="enableSsl">是否启用SSL加密</param>
        /// <param name="userName">登录帐号</param>
        /// <param name="pwd">登录密码</param>
        /// <param name="nickName">发件人昵称</param>
        /// <param name="fromEmail">发件人</param>
        /// <param name="toEmail">收件人</param>
        /// <param name="subj">主题</param>
        /// <param name="bodys">内容</param>
        public static async Task SendMailAsync(MailSendModel sendModel, Dictionary<string, Stream> attachFiles = null)
        {
            await Task.Run(() =>
            {
                try
                {
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network; //指定电子邮件发送方式
                    smtpClient.Host = sendModel.SendConfig.SmtpHost; //指定SMTP服务器
                    smtpClient.Credentials = new NetworkCredential(sendModel.SendConfig.SmtpUserName, sendModel.SendConfig.SmtpPassword); //用户名和密码
                    smtpClient.EnableSsl = sendModel.SendConfig.EnableSsl;
                    MailMessage mailMessage = new MailMessage();
                    MailAddress fromAddress = new MailAddress(sendModel.FromUser.UserAddress, sendModel.FromUser.UserName);
                    mailMessage.From = fromAddress;
                    foreach (var toMail in sendModel.ToUsers)
                    {
                        MailAddress toAddress = new MailAddress(toMail.UserAddress, toMail.UserName);
                        mailMessage.To.Add(toAddress);
                    }

                    foreach (var ccMail in sendModel.CcUsers)
                    {
                        MailAddress ccAddress = new MailAddress(ccMail.UserAddress, ccMail.UserName);
                        mailMessage.CC.Add(ccAddress);
                    }

                    mailMessage.Subject = sendModel.Subject; //主题
                    mailMessage.Body = sendModel.Content; //内容
                    mailMessage.BodyEncoding = Encoding.UTF8; //正文编码
                    mailMessage.IsBodyHtml = true; //设置为HTML格式
                    mailMessage.Priority = MailPriority.Normal; //优先级

                    if (attachFiles != null)
                    {
                        foreach (var key in attachFiles.Keys)
                        {
                            string fileName = key;
                            string fileformat = fileName.Split('.')[fileName.Split('.').Length - 1];
                            string contentType = FileMimeType.GetMimeType(fileformat);
                            Attachment data = new Attachment(fileName, MediaTypeNames.Application.Octet);
                            System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;
                            disposition.CreationDate = System.IO.File.GetCreationTime(fileName);
                            disposition.ModificationDate = System.IO.File.GetLastWriteTime(fileName);
                            disposition.ReadDate = System.IO.File.GetLastAccessTime(fileName);
                            mailMessage.Attachments.Add(data);
                        }
                    }
                    smtpClient.SendMailAsync(mailMessage);
                }
                catch (System.Exception ex)
                {
                    Log.Warning(ex.Message);
                }
            });
        }
    }
}