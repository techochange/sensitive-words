using System;
using System.Collections.Generic;
using System.Text;

namespace BMT.Server.Infrastructure
{
    /// <summary>
    /// GUID生成器
    /// </summary>
    public static class GuidHelper
    {
        /// <summary>
        /// 生成具有时序性的GUID
        /// </summary>
        /// <returns>GUID</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///  Guid guid=GuidHelper.NewComb();
        /// ]]>
        /// </code>
        /// </example>
        public static Guid NewComb()
        {
            byte[] guidArray = System.Guid.NewGuid().ToByteArray();
            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;
            // Get the days and milliseconds which will be used to build the byte string 
            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = new TimeSpan(now.Ticks - (new DateTime(now.Year, now.Month, now.Day).Ticks));

            // Convert to a byte array 
            // Note that sql server is accurate to 1/300th of a millisecond so we divide by 3.333333 
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match sql servers ordering 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid 
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new System.Guid(guidArray);
        }

        /// <summary> 
        ///  将具有时序性GUID 转换成时间信息 
        /// </summary> 
        /// <param name="guid">包含时间信息的 COMB </param> 
        /// <returns>时间</returns> 
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///  DateTime guid=GuidHelper.NewComb().GetDateFromComb(); //return DateTime 
        /// ]]>
        /// </code>
        /// </example>
        public static DateTime GetDateFromComb(this Guid guid)
        {
            DateTime baseDate = new DateTime(1900, 1, 1);
            byte[] daysArray = new byte[4];
            byte[] msecsArray = new byte[4];
            byte[] guidArray = guid.ToByteArray();

            // Copy the date parts of the guid to the respective byte arrays. 
            Array.Copy(guidArray, guidArray.Length - 6, daysArray, 2, 2);
            Array.Copy(guidArray, guidArray.Length - 4, msecsArray, 0, 4);

            // Reverse the arrays to put them into the appropriate order 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Convert the bytes to ints 
            int days = BitConverter.ToInt32(daysArray, 0);
            int msecs = BitConverter.ToInt32(msecsArray, 0);

            DateTime date = baseDate.AddDays(days);
            date = date.AddMilliseconds(msecs * 3.333333);

            return date;
        }

        /// <summary>
        /// 将Guid转换为 int64的数字
        /// </summary>
        /// <param name="guid">待转换的guid</param>
        /// <returns>long值</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///  long value=Guid.NewGuid().ToLong();
        /// ]]>
        /// </code>
        /// </example>
        public static long ToLong(this Guid guid)
        {
            byte[] bytes = guid.ToByteArray();
            return BitConverter.ToInt64(bytes, 0);
        }

        /// <summary>
        /// 生成一个新的GUID
        /// </summary>
        /// <returns>生成的guid</returns>
        public static string NewOne()
        {
            return System.Guid.NewGuid().ToString().Replace("-", "").ToLower();
        }
    }
}
