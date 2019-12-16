using Badmati.BinarySpace.Infrastructure;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AliyunTest
{
    class Program
    {
        public static void Main(string[] args)
        {

            for (var i = 0; i < 50; i++)
            {
                //var id = new Random().Next(99999999, 999999999);
                var data = IDCodeHelper.IdToCode12(i);
                //var tick = DateTime.Now.Ticks;
                ////DateTime.Now.GetTimeStamp()
                Console.WriteLine(i);
                Console.WriteLine(data);


                Console.WriteLine(IDCodeHelper.Code12ToId(data));
            }
            
            Console.ReadKey();
            //TestWechatPay();
        }

        static void TestWechatPay()
        {
            var wechat = new WechatOrder
            {
                appid = "wx07e0f79b49824a89",
                body = "真爱永恒专属证书费",
                device_info = "5201314",
                mch_id = "1554882681",
                nonce_str = Guid.NewGuid().ToString().Replace("-", ""),
                notify_url = "https://www.badmati.cn/aa/values",
                openid = "oQoyL5TbhSr7lH-jyB1qsV4uI0AQ",
                out_trade_no = GuidHelper.NewComb().ToString().Replace("-", ""),
                sign_type = "MD5",
                spbill_create_ip = "219.152.31.151",
                total_fee = 1314,
                trade_type = "JSAPI"
            };
            wechat.sign = WechatPayHelper.GetOrderSign(wechat,string.Empty);

            WriteLog(wechat.Json2Str());
            var data = XmlHelper.Serialize(wechat);

            WriteLog(data);
            var result = WechatPayHelper.DoMakeOrder(wechat).Result;
            //result.Wait();
            //var text = result.Result;
            WriteLog(result.Json2Str());

            Console.ReadKey();
        }

        static void TestAliValid()
        {
            var result = AliyunHelper.DoValidate(new UserValidInput { IDNumber = "120120198801011231", UserName = "柳传志", MobilePhone = "13800138000" });
            if (result != null)
            {

                WriteLog(result.Json2Str(false));
            }
            Console.WriteLine("\n");
            Console.ReadKey();
        }
        private static void WriteLog(string text)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                path = System.IO.Path.Combine(path
                , "UserValid\\");

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                string fileFullName = System.IO.Path.Combine(path
                , string.Format("{0}.txt", DateTime.Now.ToString("yyyyMMdd")));


                using (StreamWriter output = System.IO.File.AppendText(fileFullName))
                {
                    output.WriteLine(text);

                    output.Close();
                }

            }
            catch (Exception ex)
            {

                //throw;
            }


        }
    }
}
