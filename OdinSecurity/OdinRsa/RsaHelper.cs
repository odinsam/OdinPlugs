using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using OdinPlugs.OdinSecurity.OdinRsa.OdinRsaEnum;
using OdinPlugs.OdinSecurity.OdinRsa.OdinRsaExtensions;

namespace OdinPlugs.OdinSecurity.OdinRsa
{
    public class RsaHelper
    {

        /// <summary>
        /// rsa加密 （分段加密） 
        /// </summary>
        /// <param name="rawInput">需要加密的原文</param>
        /// <param name="publicKey">加密所使用的公钥publicKey</param>
        /// <returns></returns>
        public static string RsaEncrypt(string rawInput, string publicKey)
        {
            if (string.IsNullOrEmpty(rawInput))
            {
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(publicKey))
            {
                throw new ArgumentException("Invalid Public Key");
            }

            using (var rsaProvider = RSA.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(rawInput); //有含义的字符串转化为字节流
                publicKey = XC.RSAUtil.RsaKeyConvert.PublicKeyPemToXml(publicKey);
                rsaProvider.FromXmlStringEx(publicKey); //载入公钥
                int bufferSize = (rsaProvider.KeySize / 8) - 11; //单块最大长度
                var buffer = new byte[bufferSize];
                using (MemoryStream inputStream = new MemoryStream(inputBytes),
                    outputStream = new MemoryStream())
                {
                    while (true)
                    { //分段加密
                        int readSize = inputStream.Read(buffer, 0, bufferSize);
                        if (readSize <= 0)
                        {
                            break;
                        }
                        var temp = new byte[readSize];
                        Array.Copy(buffer, 0, temp, 0, readSize);
                        var encryptedBytes = rsaProvider.Encrypt(temp, RSAEncryptionPadding.Pkcs1);
                        outputStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                    }
                    return Convert.ToBase64String(outputStream.ToArray()); //转化为字节流方便传输
                }
            }
        }

        /// <summary>
        /// rsa解密
        /// </summary>
        /// <param name="encryptedInput">需要解密的密文</param>
        /// <param name="privateKey">解密所使用的私钥privateKye(PKCS8格式)</param>
        /// <returns></returns>
        public static string RsaDecrypt(string encryptedInput, string privateKey, EnumRsaKeyType keyType)
        {
            encryptedInput = encryptedInput.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+").Replace("#", "").Replace("\\", "");
            if (string.IsNullOrEmpty(encryptedInput))
            {
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(privateKey))
            {
                throw new ArgumentException("Invalid Private Key");
            }

            using (var rsaProvider = RSA.Create())
            {
                var inputBytes = Convert.FromBase64String(encryptedInput);
                rsaProvider.FromXmlStringEx(RsaKeyTypeConvert.ConvertPrivateKey(privateKey, keyType));
                int bufferSize = rsaProvider.KeySize / 8;
                var buffer = new byte[bufferSize];
                using (MemoryStream inputStream = new MemoryStream(inputBytes),
                    outputStream = new MemoryStream())
                {
                    while (true)
                    {
                        int readSize = inputStream.Read(buffer, 0, bufferSize);
                        if (readSize <= 0)
                        {
                            break;
                        }

                        var temp = new byte[readSize];
                        Array.Copy(buffer, 0, temp, 0, readSize);
                        var rawBytes = rsaProvider.Decrypt(temp, RSAEncryptionPadding.Pkcs1);
                        outputStream.Write(rawBytes, 0, rawBytes.Length);
                    }
                    return Encoding.UTF8.GetString(outputStream.ToArray());
                }
            }
        }
    }
}