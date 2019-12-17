using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BMT.Server.Infrastructure
{

    /// <summary>
    /// 处理微信小程序用户数据的签名验证和解密
    /// </summary>
    public class WeChatAppDecrypt
    {
        private string appId;
        private string appSecret;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appId">应用程序的AppId</param>
        /// <param name="appSecret">应用程序的AppSecret</param>
        public WeChatAppDecrypt(string appId, string appSecret)
        {
            this.appId = appId;
            this.appSecret = appSecret;
            return;
        }

        /// <summary>
        /// 获取OpenId和SessionKey的Json数据包
        /// </summary>
        /// <param name="code">客户端发来的code</param>
        /// <returns>Json数据包</returns>
        private async Task<string> GetOpenIdAndSessionKeyString(string code)
        {
            string temp = "https://api.weixin.qq.com/sns/jscode2session?" +
                "appid=" + appId
                + "&secret=" + appSecret
                + "&js_code=" + code
                + "&grant_type=authorization_code";

            return await GetResponse(temp);

        }

        /// <summary>
        /// 反序列化包含OpenId和SessionKey的Json数据包
        /// </summary>
        /// <param name="code">Json数据包</param>
        /// <returns>包含OpenId和SessionKey的类</returns>
        public async Task<OpenIdAndSessionKey> DecodeOpenIdAndSessionKey(WechatLoginInfo loginInfo)
        {
            OpenIdAndSessionKey oiask = (await GetOpenIdAndSessionKeyString(loginInfo.code)).Str2Json<OpenIdAndSessionKey>();
            if (!String.IsNullOrEmpty(oiask.errcode))
                return null;
            return oiask;
        }

        /// <summary>
        /// 根据微信小程序平台提供的签名验证算法验证用户发来的数据是否有效
        /// </summary>
        /// <param name="rawData">公开的用户资料</param>
        /// <param name="signature">公开资料携带的签名信息</param>
        /// <param name="sessionKey">从服务端获取的SessionKey</param>
        /// <returns>True：资料有效，False：资料无效</returns>
        public bool VaildateUserInfo(string rawData, string signature, string sessionKey)
        {
            //创建SHA1签名类
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            //编码用于SHA1验证的源数据
            byte[] source = Encoding.UTF8.GetBytes(rawData + sessionKey);
            //生成签名
            byte[] target = sha1.ComputeHash(source);
            //转化为string类型，注意此处转化后是中间带短横杠的大写字母，需要剔除横杠转小写字母
            string result = BitConverter.ToString(target).Replace("-", "").ToLower();
            //比对，输出验证结果
            return signature == result;
        }

        /// <summary>
        /// 根据微信小程序平台提供的签名验证算法验证用户发来的数据是否有效
        /// </summary>
        /// <param name="loginInfo">登陆信息</param>
        /// <param name="sessionKey">从服务端获取的SessionKey</param>
        /// <returns>True：资料有效，False：资料无效</returns>
        public bool VaildateUserInfo(WechatLoginInfo loginInfo, string sessionKey)
        {
            return VaildateUserInfo(loginInfo.rawData, loginInfo.signature, sessionKey);
        }

        /// <summary>
        /// 根据微信小程序平台提供的签名验证算法验证用户发来的数据是否有效
        /// </summary>
        /// <param name="loginInfo">登陆信息</param>
        /// <param name="idAndKey">包含OpenId和SessionKey的类</param>
        /// <returns>True：资料有效，False：资料无效</returns>
        public bool VaildateUserInfo(WechatLoginInfo loginInfo, OpenIdAndSessionKey idAndKey)
        {
            return VaildateUserInfo(loginInfo, idAndKey.session_key);
        }

        /// <summary>
        /// 根据微信小程序平台提供的解密算法解密数据
        /// </summary>
        /// <param name="encryptedData">加密数据</param>
        /// <param name="iv">初始向量</param>
        /// <param name="sessionKey">从服务端获取的SessionKey</param>
        /// <returns></returns>
        public WechatUserInfo Decrypt(string encryptedData, string iv, string sessionKey)
        {
            WechatUserInfo userInfo;
            //创建解密器生成工具实例
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            //设置解密器参数
            aes.Mode = CipherMode.CBC;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.PKCS7;
            //格式化待处理字符串
            byte[] byte_encryptedData = Convert.FromBase64String(encryptedData);
            byte[] byte_iv = Convert.FromBase64String(iv);
            byte[] byte_sessionKey = Convert.FromBase64String(sessionKey);

            aes.IV = byte_iv;
            aes.Key = byte_sessionKey;
            //根据设置好的数据生成解密器实例
            ICryptoTransform transform = aes.CreateDecryptor();

            //解密
            byte[] final = transform.TransformFinalBlock(byte_encryptedData, 0, byte_encryptedData.Length);

            //生成结果
            string result = Encoding.UTF8.GetString(final);

            //反序列化结果，生成用户信息实例
            userInfo = result.Str2Json<WechatUserInfo>();

            return userInfo;

        }

        /// <summary>
        /// 根据微信小程序平台提供的解密算法解密数据
        /// </summary>
        /// <param name="encryptedData">加密数据</param>
        /// <param name="iv">初始向量</param>
        /// <param name="sessionKey">从服务端获取的SessionKey</param>
        /// <returns></returns>
        public string DecryptString(string encryptedData, string iv, string sessionKey)
        {/*
            
             */

            //创建解密器生成工具实例
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            //设置解密器参数
            aes.Mode = CipherMode.CBC;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.PKCS7;
            //Cipher cipher = Cipher.getInstance("AES/CBC/PKCS7Padding");
            encryptedData = encryptedData.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+");
            if (encryptedData.Length % 4 > 0)
            {
                encryptedData = encryptedData.PadRight(encryptedData.Length + 4 - encryptedData.Length % 4, '=');
            }
            //格式化待处理字符串
            byte[] byte_encryptedData = Convert.FromBase64String(encryptedData);
            byte[] byte_iv = Convert.FromBase64String(iv);
            byte[] byte_sessionKey = Convert.FromBase64String(sessionKey);

            aes.IV = byte_iv;
            aes.Key = byte_sessionKey;
            //根据设置好的数据生成解密器实例
            ICryptoTransform transform = aes.CreateDecryptor();

            //解密
            byte[] final = transform.TransformFinalBlock(byte_encryptedData, 0, byte_encryptedData.Length);

            //生成结果
            string result = Encoding.UTF8.GetString(final);
            return result;

        }

        public string AES_decrypt(string encryptedData, string iv, string sessionKey)
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 128;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Convert.FromBase64String(sessionKey);
            aes.IV = Convert.FromBase64String(iv);
            var decrypt = aes.CreateDecryptor();
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Convert.FromBase64String(encryptedData);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }
            string Output = Encoding.UTF8.GetString(xBuff);
            return Output;
        }

        public string WXAES_decrypt(string encryptedDataStr, string key, string iv)
        {

            //创建解密器生成工具实例
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            //设置解密器参数
            aes.Mode = CipherMode.CBC;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.PKCS7;
            //Cipher cipher = Cipher.getInstance("AES/CBC/PKCS7Padding");
            //格式化待处理字符串
            byte[] byte_encryptedData = Convert.FromBase64String(encryptedDataStr);
            byte[] byte_iv = Convert.FromBase64String(iv);
            byte[] byte_sessionKey = Convert.FromBase64String(key);

            aes.IV = byte_iv;
            aes.Key = byte_sessionKey;
            



            string result;
            try
            {
                result = AES_decrypt(encryptedDataStr, iv, key);
            }
            catch (Exception ex)
            {
                try
                {

                    aes.Padding = PaddingMode.Zeros;
                    //根据设置好的数据生成解密器实例
                    ICryptoTransform transform = aes.CreateDecryptor();
                    //解密
                    byte[] final = transform.TransformFinalBlock(byte_encryptedData, 0, byte_encryptedData.Length);
                    result = Encoding.UTF8.GetString(final);
                    var ss = Encoding.Unicode.GetString(final);
                    var sdk = Encoding.UTF32.GetString(final);
                }
                catch (Exception ex1)
                {

                    try
                    {

                        aes.Padding = PaddingMode.ANSIX923;
                        //根据设置好的数据生成解密器实例
                        ICryptoTransform transform = aes.CreateDecryptor();
                        //解密
                        byte[] final = transform.TransformFinalBlock(byte_encryptedData, 0, byte_encryptedData.Length);
                        result = Encoding.UTF8.GetString(final);
                    }
                    catch (Exception ex2)
                    {
                        try
                        {

                            aes.Padding = PaddingMode.ISO10126;
                            //根据设置好的数据生成解密器实例
                            ICryptoTransform transform = aes.CreateDecryptor();
                            //解密
                            byte[] final = transform.TransformFinalBlock(byte_encryptedData, 0, byte_encryptedData.Length);
                            result = Encoding.UTF8.GetString(final);

                        }
                        catch (Exception ex3)
                        {
                            try
                            {

                                aes.Padding = PaddingMode.None;
                                //根据设置好的数据生成解密器实例
                                ICryptoTransform transform = aes.CreateDecryptor();
                                //解密
                                byte[] final = transform.TransformFinalBlock(byte_encryptedData, 0, byte_encryptedData.Length);
                                result = Encoding.UTF8.GetString(final);
                            }
                            catch (Exception ex4)
                            {
                                result = string.Empty;
                            }

                        }
                    }
                }
            }
            
            return result;
        }

        /// <summary>
        /// 根据微信小程序平台提供的解密算法解密数据，推荐直接使用此方法
        /// </summary>
        /// <param name="loginInfo">登陆信息</param>
        /// <returns>用户信息</returns>
        public async Task<WechatUserInfo> Decrypt(WechatLoginInfo loginInfo)
        {
            if (loginInfo == null)
                return null;

            if (String.IsNullOrEmpty(loginInfo.code))
                return null;

            OpenIdAndSessionKey oiask = await DecodeOpenIdAndSessionKey(loginInfo);

            if (oiask == null)
                return null;

            if (!VaildateUserInfo(loginInfo, oiask))
                return null;

            WechatUserInfo userInfo = Decrypt(loginInfo.encryptedData, loginInfo.iv, oiask.session_key);

            return userInfo;
        }

        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<string> GetResponse(string url)
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string result =await response.Content.ReadAsStringAsync();
                return result;
            }
            return null;
        }


    }
    /// <summary>
    /// 微信小程序登录信息结构
    /// </summary>
    public class WechatLoginInfo
    {
        public string code { get; set; }
        public string encryptedData { get; set; }
        public string iv { get; set; }
        public string rawData { get; set; }
        public string signature { get; set; }
    }

    public class WechatShareInfo
    {
        public string session_key { get; set; }
        public string openId { get; set; }
        public string code { get; set; }
        public string errmsg { get; set; }
        public string iv { get; set; }
        public string encryptedData { get; set; }
    }

    public class WechatGroupInfo
    {
        public string openGId { get; set; }
    }
    /// <summary>
    /// 微信小程序用户信息结构
    /// </summary>
    public class WechatUserInfo
    {
        public string openId { get; set; }
        public string nickName { get; set; }
        public string gender { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string avatarUrl { get; set; }
        public string unionId { get; set; }
        public Watermark watermark { get; set; }

        public class Watermark
        {
            public string appid { get; set; }
            public string timestamp { get; set; }
        }
    }
    /// <summary>
    /// 微信小程序从服务端获取的OpenId和SessionKey信息结构
    /// </summary>
    public class OpenIdAndSessionKey:OpenId
    {
        public string session_key { get; set; }
    }

    public class OpenId
    {
        public string openid { get; set; }
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public OpenId() { }
        public OpenId(OpenIdAndSessionKey dto)
        {
            this.openid = dto.openid;
            this.errcode = dto.errcode;
            this.errmsg = dto.errmsg;
        }
    }
}
