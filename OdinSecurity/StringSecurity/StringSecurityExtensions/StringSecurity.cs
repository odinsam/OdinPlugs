using System.Collections.Generic;
using System.Threading;
using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OdinPlugs.OdinSecurity.StringSecurity.StringSecurityExtensions
{
    public static class StringSecurity
    {
        public static JObject AddWxNameMark(this Object entity, string fieldName, string newfieldName = null)
        {
            var wxName = entity.GetType().GetProperty(fieldName).GetValue(entity).ToString();
            string wxNameMark = null;
            var star = "";
            for (int i = 0; i < wxName.Length - 2; i++)
            {
                star += "*";
            }
            wxNameMark = wxName.Substring(0, 2) + star;
            var jobj = JObject.Parse(JsonConvert.SerializeObject(entity));
            jobj.Add(newfieldName ?? $"Mark{fieldName}", wxNameMark);
            return jobj;
        }

        public static JObject AddMailMark(this Object entity, string fieldName, string newfieldName = null)
        {
            var mail = entity.GetType().GetProperty(fieldName).GetValue(entity).ToString().Split('@')[0].ToString();
            string mailMark = null;
            if (mail.Length > 3)
            {
                var star = "";
                for (int i = 0; i < mail.Length - 2; i++)
                {
                    star += "*";
                }
                mailMark = mail.Substring(0, 1) + star + mail.Substring(mail.Length - 1, 1);
            }
            else
            {
                mailMark = mail;
            }
            var jobj = JObject.Parse(JsonConvert.SerializeObject(entity));
            jobj.Add(newfieldName ?? $"Mark{fieldName}", mailMark);
            return jobj;
        }

        public static JObject AddNameMark(this Object entity, string fieldName, string newfieldName = null)
        {
            var userName = entity.GetType().GetProperty(fieldName).GetValue(entity).ToString();
            string userNameMark = null;
            if (userName.Length == 2)
            {
                userNameMark = userName.Substring(0, 1) + "*";
            }
            else if (userName.Length == 3)
            {
                userNameMark = userName.Substring(0, 1) + "*" + userName.Substring(2, 1);
            }
            else
            {
                var star = "";
                for (int i = 0; i < userName.Length - 2; i++)
                {
                    star += "*";
                }
                userNameMark = userName.Substring(0, 1) + star + userName.Substring(userName.Length - 1, 1);
            }
            var jobj = JObject.Parse(JsonConvert.SerializeObject(entity));
            jobj.Add(newfieldName ?? $"Mark{fieldName}", userNameMark);
            return jobj;
        }


        public static JObject AddCardIdMark(this Object entity, string fieldName, string newfieldName = null)
        {
            var cardId = entity.GetType().GetProperty(fieldName).GetValue(entity).ToString();
            var cardIdMark = cardId.Substring(0, 10) + "*******" + cardId.Substring(cardId.Length - 1, 1);
            var jobj = JObject.Parse(JsonConvert.SerializeObject(entity));
            jobj.Add(newfieldName ?? $"Mark{fieldName}", cardIdMark);
            return jobj;
        }

        public static JObject AddPhoneMark(this Object entity, string fieldName, string newfieldName = null)
        {
            var phone = entity.GetType().GetProperty(fieldName).GetValue(entity).ToString();
            var phoneMark = phone.Substring(0, 3) + "****" + phone.Substring(7);
            var jobj = JObject.Parse(JsonConvert.SerializeObject(entity));
            jobj.Add(newfieldName ?? $"Mark{fieldName}", phoneMark);
            return jobj;
        }
        public static string ToMd5Lower(this string str, int length = 32)
        {
            MD5 md5 = MD5.Create();
            byte[] bt = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bt.Length; i++)
            {
                sb.AppendFormat("{0:x2}", bt[i]);
            }
            return sb.ToString().ToLower().Substring(0, length);
        }

        public static string ToMd5Upper(this string str, int length = 32)
        {
            MD5 md5 = MD5.Create();
            byte[] bt = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bt.Length; i++)
            {
                sb.AppendFormat("{0:x2}", bt[i]);
            }
            return sb.ToString().ToUpper().Substring(0, length);
        }

    }
}