using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BMT.Server.Infrastructure
{
    /// <summary>
    /// 字符加解密工具类
    /// </summary>
    public static class SecurityHelper
    {
        private static string _wnlWord = "ylWnl";
        #region MD5加密

        /// <summary>
        /// 返回 System.String 对象进行MD5加密后的32字符十六进制格式字符串
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <param name="encode">编码</param>
        /// <returns>返回加密字符串</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///  string md5="1234".ToMD5(Encoding.UTF8);
        ///  //返回字符长度为32
        /// ]]>
        /// </code>
        /// </example>
        public static string ToMD5(this string str, Encoding encode)
        {
            if (str == null)
                throw new ArgumentNullException("str");


            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = md5.ComputeHash(encode.GetBytes(str));
                StringBuilder sb = new StringBuilder();
                foreach (var i in bytes)
                {
                    sb.Append(i.ToString("x2"));
                }
                return sb.ToString().ToUpper();
            }
        }
        /// <summary>
        /// 返回 System.String 对象进行MD5加密后的32字符十六进制格式字符串,默认编码UTF8
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>返回加密字符串</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///  string md5="1234".ToMD5(); // default encoding is  Encoding.UTF8
        ///  //返回字符长度为32
        /// ]]>
        /// </code>
        /// </example>
        public static string ToMD5(this string str)
        {
            return str.ToMD5(Encoding.UTF8);
        }
        /// <summary>
        /// 返回 System.String 对象进行MD5加密后的16字符十六进制格式字符串
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <param name="encode">编码</param>
        /// <returns>返回加密字符串</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///  string md5="1234".ToMD5Bit16(Encoding.UTF8); //返回字符长度为16
        /// ]]>
        /// </code>
        /// </example>
        public static string ToMD5Bit16(this string str, Encoding encode)
        {
            return str.ToMD5(encode).Substring(8, 16);
        }

        /// <summary>
        /// 返回 System.String 对象进行MD5加密后的16字符十六进制格式字符串,默认编码GB2312
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>返回加密字符串</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///  string md5="1234".ToMD5Bit16(); //default encoding is  Encoding.UTF8 ,返回字符长度为16
        /// ]]>
        /// </code>
        /// </example>
        public static string ToMD5Bit16(this string str)
        {
            return str.ToMD5Bit16(Encoding.UTF8);
        }
        #endregion

        #region Base64加密
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="str">待加密字符串</param>
        /// <param name="encode">字符编码，默认UTD-8</param>
        /// <returns>Base64后的字符串</returns>
        public static string ToBase64Encode(this string str, string encode = "UTF-8")
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            return Convert.ToBase64String(Encoding.GetEncoding(encode).GetBytes(str));
        }
        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="str">待解密字符串</param>
        /// <param name="encode">字符编码，默认UTD-8</param>
        /// <returns>Base64解密后的字符串</returns>
        public static string FromBase64String(this string str, string encode = "UTF-8")
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            try
            {
                return Encoding.GetEncoding(encode).GetString(Convert.FromBase64String(str));
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region AES加密
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="str">待加密字符串</param>
        /// <param name="key">密钥(长度16位)</param>
        /// <returns>加密后的字符串 (BASE64)</returns>
        public static string ToAesEncrypt(this string str, string key)
        {
#if NETCORE
            using (Aes aes = Aes.Create())
#else
            using (var aes = new RijndaelManaged())
#endif
            {
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(key);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform cryptoTransform = aes.CreateEncryptor())
                {
                    var bResult = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);

                    return Convert.ToBase64String(bResult); //返回base64加密; 
                }
            }

        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="str">待解密字符串 Base64 （UTF-8）</param>
        /// <param name="key">密钥(长度16位)</param>
        /// <returns>解密后的字符串</returns>
        public static string ToAesDecrypt(this string str, string key)
        {
#if NETCORE
            using (Aes aes = Aes.Create())
#else
            using (var aes = new RijndaelManaged())
#endif
            {
                byte[] bytes = Convert.FromBase64String(str); //解密base64;   
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(key);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform cryptoTransform = aes.CreateDecryptor())
                {
                    var bResult = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
                    return Encoding.UTF8.GetString(bResult);
                }
            }
        }
        #endregion

        #region DES加解密

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="text">需解密的密文</param>
        /// <param name="key">密钥</param>
        /// <returns>DES解密的后的字符串</returns>
        public static string ToDesDecrypt(this string text, string key)
        {
#if NETSTANDARD1_6
            throw new NotSupportedException("暂不支持DES解密算法,建议使用AES算法替换DES算法");
#else
            DESCryptoServiceProvider provider1 = new DESCryptoServiceProvider();
            int num1 = text.Length / 2;
            byte[] buffer1 = new byte[num1];
            for (int num2 = 0; num2 < num1; num2++)
            {
                int num3 = Convert.ToInt32(text.Substring(num2 * 2, 2), 0x10);
                buffer1[num2] = (byte)num3;
            }
            provider1.Key = Encoding.ASCII.GetBytes(key.ToMD5().Substring(0, 8));
            provider1.IV = Encoding.ASCII.GetBytes(key.ToMD5().Substring(0, 8));

            MemoryStream stream1 = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream1, provider1.CreateDecryptor(), CryptoStreamMode.Write);

            stream2.Write(buffer1, 0, buffer1.Length);
            stream2.FlushFinalBlock();
            return Encoding.Default.GetString(stream1.ToArray());
#endif
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="str">需加密的明文文本</param>
        /// <param name="key">密钥</param>
        /// <returns>DES加密后的字符串</returns>
        public static string ToDesEncrypt(this string str, string key)
        {
#if NETSTANDARD1_6
            throw new NotSupportedException("暂不支持DES加密算法,建议使用AES算法替换DES算法");
#else
            DESCryptoServiceProvider provider1 = new DESCryptoServiceProvider();
            byte[] buffer1 = Encoding.Default.GetBytes(str);
            provider1.Key = Encoding.ASCII.GetBytes(key.ToMD5().Substring(0, 8));
            provider1.IV = Encoding.ASCII.GetBytes(key.ToMD5().Substring(0, 8));
            MemoryStream stream1 = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream1, provider1.CreateEncryptor(), CryptoStreamMode.Write);
            stream2.Write(buffer1, 0, buffer1.Length);
            stream2.FlushFinalBlock();
            StringBuilder builder1 = new StringBuilder();
            foreach (byte num1 in stream1.ToArray())
            {
                builder1.AppendFormat("{0:X2}", num1);
            }
            return builder1.ToString();
#endif
        }
        #endregion

        #region RSA加解密

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="encryptString">需要加密的文本</param>
        /// <param name="xmlPublicKey">加密公钥</param>
        /// <returns>RSA公钥加密后的数据</returns>
        public static string ToRSAEncrypt(this string encryptString, string xmlPublicKey)
        {
            string result;
            try
            {
                byte[] bytes = new UnicodeEncoding().GetBytes(encryptString);
                byte[] resBytes = null;
#if NETCORE
                using (RSA rsa = RSA.Create())
                {
                    rsa.FromXmlStringEx(xmlPublicKey);
                    resBytes = rsa.Encrypt(bytes, RSAEncryptionPadding.Pkcs1);
                }
#else
                RSACryptoServiceProvider.UseMachineKeyStore = true;
                RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
                provider.FromXmlString(xmlPublicKey);
                resBytes = provider.Encrypt(bytes, false);
#endif

                result = Convert.ToBase64String(resBytes);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return result;
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="decryptString">需要解密的文本</param>
        /// <param name="xmlPrivateKey">解密私钥</param>
        /// <returns>解密后的数据</returns>
        public static string ToRSADecrypt(this string decryptString, string xmlPrivateKey)
        {
            string result;
            try
            {
                byte[] bytes = Convert.FromBase64String(decryptString);
                byte[] resBytes = null;
#if NETCORE
                using (RSA rsa = RSA.Create())
                {
                    rsa.FromXmlStringEx(xmlPrivateKey);
                    resBytes = rsa.Decrypt(bytes, RSAEncryptionPadding.Pkcs1);
                }
#else
                RSACryptoServiceProvider.UseMachineKeyStore = true;
                RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
                provider.FromXmlString(xmlPrivateKey);
                resBytes = provider.Decrypt(bytes, false);
#endif
                result = new UnicodeEncoding().GetString(resBytes);
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return result;
        }

        /// <summary>
        /// 生成RSA公钥、私钥
        /// </summary>
        /// <param name="privateKey">私钥</param>
        /// <param name="publicKey">公钥</param>
        public static void CreateRSAKey(out string privateKey, out string publicKey)
        {
#if NETCORE
            using (RSA rsa = RSA.Create())
            {
                privateKey = rsa.ToXmlStringEx(true);
                publicKey = rsa.ToXmlStringEx(false);
            }
#else
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                privateKey = rsa.ToXmlString(true);
                publicKey = rsa.ToXmlString(false);
            }
#endif
        }

        #endregion

        #region ShA1

        /// <summary>
        /// sha1加密 用户用户密码加密防止拖库造成用户密码泄漏
        /// </summary>
        /// <param name="normalTxt"></param>
        /// <returns></returns>
        public static string Sha1Encrypt(string normalTxt)
        {
            normalTxt = _wnlWord + normalTxt;
            var bytes = Encoding.UTF8.GetBytes(normalTxt);
            var SHA = new SHA1CryptoServiceProvider();
            var encryptbytes = SHA.ComputeHash(bytes);
            var sb = new StringBuilder();
            foreach (var t in encryptbytes)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString();
        }
        #endregion

    }

#if NETCORE
    internal static class RSAExtentions
    {
        public static void FromXmlStringEx(this RSA rsa, string xmlString)
        {
            RSAParameters parameters = new RSAParameters();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);
            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": parameters.Modulus = Convert.FromBase64String(node.InnerText); break;
                        case "Exponent": parameters.Exponent = Convert.FromBase64String(node.InnerText); break;
                        case "P": parameters.P = Convert.FromBase64String(node.InnerText); break;
                        case "Q": parameters.Q = Convert.FromBase64String(node.InnerText); break;
                        case "DP": parameters.DP = Convert.FromBase64String(node.InnerText); break;
                        case "DQ": parameters.DQ = Convert.FromBase64String(node.InnerText); break;
                        case "InverseQ": parameters.InverseQ = Convert.FromBase64String(node.InnerText); break;
                        case "D": parameters.D = Convert.FromBase64String(node.InnerText); break;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }

            rsa.ImportParameters(parameters);
        }

        public static string ToXmlStringEx(this RSA rsa, bool includePrivateParameters)
        {
            RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);

            if (includePrivateParameters)
            {
                return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                    Convert.ToBase64String(parameters.Modulus),
                    Convert.ToBase64String(parameters.Exponent),
                    Convert.ToBase64String(parameters.P),
                    Convert.ToBase64String(parameters.Q),
                    Convert.ToBase64String(parameters.DP),
                    Convert.ToBase64String(parameters.DQ),
                    Convert.ToBase64String(parameters.InverseQ),
                    Convert.ToBase64String(parameters.D));
            }
            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
                    Convert.ToBase64String(parameters.Modulus),
                    Convert.ToBase64String(parameters.Exponent));
        }

    }
#endif
}
