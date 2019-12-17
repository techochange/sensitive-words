using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BMT.Server.Infrastructure
{
    /// <summary>
    /// 文件操作帮助类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 从文件中读取数据
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="encoding">编码格式</param>
        /// <returns></returns>
        public static string ReadFile(string filePath, Encoding encoding)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(stream, encoding))
                {
                    return sr.ReadToEnd().Trim();
                }
            }
        }
        /// <summary>
        /// 从文件中读取数据 ,编码Encoding.UTF8
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string ReadFile(string filePath)
        {
            return ReadFile(filePath, Encoding.UTF8);
        }

        /// <summary>
        /// 创建父级目录，如果父级目录不存在的话
        /// </summary>
        /// <param name="filePath">文件路径，比如:d:\web\config\log4net.config</param>
        /// <returns></returns>
        public static void CreateParentDirectoryIfUnExist(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// 写入文件内容，覆盖原有内容 ，编码：Encoding.UTF8
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="content">文件内容</param>
        /// <returns></returns>
        public static bool WriteFile(string filePath, string content)
        {
            return WriteFile(filePath, content, Encoding.UTF8);
        }

        /// <summary>
        /// 写入文件内容，覆盖原有内容
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="content">文件内容</param>
        /// <param name="encoding">编码格式</param>
        /// <returns></returns>
        public static bool WriteFile(string filePath, string content, Encoding encoding)
        {
            try
            {
                CreateParentDirectoryIfUnExist(filePath);
                using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(stream, encoding, 1024))
                    {
                        sw.Write(content);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string Save2File(string fileFullName, Stream srcStream)
        {
            return Save2File(fileFullName, srcStream, Encoding.UTF8);
        }
        public static string Save2File(string fileFullName, Stream srcStream, Encoding encoding)
        {
            CheckDir4File(fileFullName);
            srcStream.Position = 0L;
            byte[] buffer = new byte[srcStream.Length];
            using (BinaryReader binaryReader = new BinaryReader(srcStream, encoding))
                binaryReader.Read(buffer, 0, (int)srcStream.Length);
            using (Stream output = (Stream)new FileStream(fileFullName, FileMode.CreateNew))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(output, encoding))
                    binaryWriter.Write(buffer, 0, buffer.Length);
            }
            return fileFullName;
        }

        private static void CheckDir4File(string fileFullName)
        {
            FileInfo fileInfo = new FileInfo(fileFullName);
            if (Directory.Exists(fileInfo.DirectoryName))
                return;
            Directory.CreateDirectory(fileInfo.DirectoryName);
        }
    }
}
