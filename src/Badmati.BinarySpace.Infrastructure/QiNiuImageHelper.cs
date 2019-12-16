//using Qiniu.Http;
//using Qiniu.Storage;
//using Qiniu.Util;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.Text;

//namespace Badmati.BinarySpace.Infrastructure
//{
//    public static class QiNiuImageHelper
//    {
//        public static string UploadImage(string localPath)
//        {
//            Mac mac = new Mac("", "");
//            string tempFileKey = "";
//            string filePath = string.Empty;
//            string bucket = string.Empty;
//            PutPolicy putPolicy = new PutPolicy();
//            putPolicy.Scope = bucket;
//            putPolicy.SetExpires(180);
//            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
//            Config config = new Config();
//            // 设置上传区域
//            config.Zone = Zone.ZONE_CN_East;
//            // 设置 http 或者 https 上传
//            config.UseHttps = true;
//            config.UseCdnDomains = true;
//            config.ChunkSize = ChunkUnit.U512K;
//            // 表单上传
//            FormUploader target = new FormUploader(config);
//            HttpResult result = target.UploadFile(filePath, tempFileKey, token, null);
//            return string.Empty;
//        }

//        public static string UploadImageStream(Stream stream, string fileName)
//        {
//            return string.Empty;
//            /*
//            Mac mac = new Mac(ConstStr.QiNiuAccesKey, ConstStr.QiNiuSecretKey);
//            string tempFileKey = fileName;
//            string bucket = ConstStr.QiNiuBucket;
//            PutPolicy putPolicy = new PutPolicy();
//            putPolicy.Scope = bucket;
//            putPolicy.SetExpires(180);
//            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
//            Config config = new Config();
//            // 设置上传区域
//            config.Zone = Zone.ZONE_CN_East;
//            // 设置 http 或者 https 上传
//            config.UseHttps = true;
//            config.UseCdnDomains = true;
//            //config.ChunkSize = ChunkUnit.U4096K;
//            // 表单上传
//            FormUploader target = new FormUploader(config);
//            HttpResult result = target.UploadStream(stream, tempFileKey, token, null);
//            if (result.Code == 200)
//            {
//                return "http://ptntup4hf.bkt.clouddn.com/" + tempFileKey;
//            }
//            return string.Empty;
//            */
//        }
//    }

//    //public static class BadmatiImageHelper
//    //{
//    //    public static string CreateImage(string folderPath, Bitmap codeImg,string code)
//    //    {
//    //        var codeImgUrl = string.Empty;
//    //        try
//    //        {
//    //            codeImgUrl = ConstStr.SERVER_URL+ConstStr.SERVER_IMG_FOLDER+ code + ".png";
//    //            codeImg.Save(folderPath + code+".png", ImageFormat.Png);
//    //            codeImg.Dispose();

//    //        }
//    //        catch (Exception ex)
//    //        {
//    //            //LogHelper.Error(typeof(ValidateCode),ex.Message,ex);
//    //        }

//    //        return codeImgUrl;
//    //    }
//    //}
//}
