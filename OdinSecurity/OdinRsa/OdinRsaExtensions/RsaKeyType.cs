using OdinPlugs.OdinSecurity.OdinRsa.OdinRsaEnum;

namespace OdinPlugs.OdinSecurity.OdinRsa.OdinRsaExtensions
{
    public class RsaKeyTypeConvert
    {
        public static string ConvertPrivateKey(string privateKey, EnumRsaKeyType keyType)
        {
            switch (keyType)
            {
                case EnumRsaKeyType.PKCS1:
                    return PrivateKey_Pkcs1ToXml(privateKey);
                case EnumRsaKeyType.PKCS8:
                    return PrivateKey_Pkcs8Toxml(privateKey);
                default:
                    return privateKey;
            }
        }
        private static string PublicKeyXmlToPem(string key)
        {
            return XC.RSAUtil.RsaKeyConvert.PublicKeyXmlToPem(key);
        }

        #region  xml->pkcs1
        public static string PrivateKey_XmlToPkcs1(string privateKey)
        {
            return XC.RSAUtil.RsaKeyConvert.PrivateKeyXmlToPkcs1(privateKey);
        }
        public static string PublicKey_XmltoPkcs1(string publicKey)
        {
            return PublicKeyXmlToPem(publicKey);
        }
        #endregion

        #region xml->pkcs8
        public static string PrivateKey_XmlToPkcs8(string privateKey)
        {
            return XC.RSAUtil.RsaKeyConvert.PrivateKeyXmlToPkcs8(privateKey);
        }
        public static string PublicKey_XmltoPkcs8(string publicKey)
        {
            return PublicKeyXmlToPem(publicKey);
        }
        #endregion

        #region Pkcs1-> XML
        public static string PrivateKey_Pkcs1ToXml(string privateKey)
        {
            return XC.RSAUtil.RsaKeyConvert.PrivateKeyPkcs1ToXml(privateKey);
        }
        public static string PublicKey_Pkcs1ToXml(string publicKey)
        {
            return PublicKeyXmlToPem(publicKey);
        }
        #endregion

        #region Pkcs1-> Pkcs8：
        public static string PrivateKey_Pkcs1ToPkcs8(string privateKey)
        {
            return XC.RSAUtil.RsaKeyConvert.PrivateKeyPkcs1ToPkcs8(privateKey);
        }
        #endregion

        #region pkcs8->xml
        public static string PrivateKey_Pkcs8Toxml(string privateKey)
        {
            return XC.RSAUtil.RsaKeyConvert.PrivateKeyPkcs8ToXml(privateKey);
        }
        public static string PublicKey_Pkcs8Toxml(string publicKey)
        {
            return PublicKeyXmlToPem(publicKey);
        }
        #endregion

        #region Pkcs8-> Pkcs1
        public static string PrivateKey_Pkcs8ToPkcs1(string privateKey)
        {
            return XC.RSAUtil.RsaKeyConvert.PrivateKeyPkcs8ToPkcs1(privateKey);
        }
        #endregion

    }
}