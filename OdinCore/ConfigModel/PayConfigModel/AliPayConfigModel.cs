namespace OdinPlugs.OdinCore.ConfigModel.PayConfigModel
{
    public class CertPath_Model
    {
        public string AppPublicKeyPath { get; set; }
        public string AlipayPublicKeyPath { get; set; }
        public string AliRootPath { get; set; }

    }
    public class AliPayCnfOptions
    {
        public string AppPublicKey { get; set; }
        public string AppId { get; set; }
        public string CharSet { get; set; }
        public string Gatewayurl { get; set; }
        public string PrivateKey { get; set; }
        public string SignType { get; set; }
        public string AlipayPublicKey { get; set; }
        public string AliNotifyUrl { get; set; }
        public string AliReturnUrl { get; set; }
        public string AliFormat { get; set; }
        public string Uid { get; set; }
        public CertPath_Model CertPath { get; set; }
    }
}