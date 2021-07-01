namespace OdinPlugs.OdinCore.ConfigModel.ApiConfigModel
{
    public class ErrorNotifyMailModel
    {
        public bool Enable { get; set; }
    }

    public class JWTModel
    {
        public bool Enable { get; set; }
        public string SecurityKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Expires { get; set; }
    }

    public class PayCnfOptions
    {
        public int orderTimeOut { get; set; } = 300;
    }

    public class WxOptions
    {
        public bool InitMenu { get; set; }
        public PayCnfOptions PayCnf { get; set; }
    }
    public class GetValidateCodeOptions
    {
        public string Url { get; set; }
        public string ApiPath { get; set; }
        public int TemplateId { get; set; }
        public string Account { get; set; }
        public string Pwd { get; set; }
    }
    public class UploadCnfOptions
    {
        public string MaxRequestBodySize { get; set; }
    }
    public class SmsApiOption
    {
        public string ApiPath { get; set; }
        public string Method { get; set; }
    }
    public class SmsApiPathOptions
    {
        public SmsApiOption SendValidateCode { get; set; }
    }
    public class SmsClientOption
    {
        public string AccountName { get; set; }
        public string AccountPwd { get; set; }
        public string Url { get; set; }
        public SmsApiPathOptions SmsApiPath { get; set; }
    }

    public class ThirdApiOption
    {
        public string Uri { get; set; }
        public string Path { get; set; }
        public string Method { get; set; }
    }

    public class BaiduOptions
    {
        public string AK { get; set; }
        public string SK { get; set; }

    }

    public class JdOptions
    {
        public string AK { get; set; }
        public string SK { get; set; }
    }
    public class AliIpSearchOptions
    {
        public string Key { get; set; }
        public string Sec { get; set; }
    }
    public class ThirdPartyApiOptions
    {
        public BaiduOptions Baidu { get; set; }
        public BaiduOptions BaiduMap { get; set; }
        public JdOptions Jd { get; set; }
        public AliIpSearchOptions AliIpSearch { get; set; }
    }

    public class ApiCnfOptions : ConfigOptions
    {
        public GetValidateCodeOptions GetValidateCode { get; set; }
        public WxOptions Wx { get; set; }

        public UploadCnfOptions UpLoad { get; set; }

        public JWTModel JWT { get; set; }

        public SmsClientOption SmsClient { get; set; }
        public ThirdPartyApiOptions ThirdPartyApi { get; set; }

    }
}