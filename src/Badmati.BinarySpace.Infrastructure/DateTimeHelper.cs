using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Badmati.BinarySpace.Infrastructure
{
    /// <summary>
    /// 对 System.DateTime的扩展
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// 将日期按照指定的格式转换为数字
        /// </summary>
        /// <param name="dateTime">待转换日期</param>
        /// <param name="format">转换格式</param>
        /// <returns>转换的结果,转换失败则返回0</returns>
        public static int ToInt(this DateTime dateTime, string format = "yyyyMMdd")
        {
            int result;
            Int32.TryParse(dateTime.ToString(format, System.Globalization.DateTimeFormatInfo.InvariantInfo), out result);
            return result;
        }
        /// <summary>
        /// 格式日期
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToYYYYMMDDStr(this DateTime dateTime, string format = "yyyyMMdd")
        {
            return dateTime.ToString(format);
        }

        /// <summary>
        /// 获取日期部分并转换为整数(yyyyMMdd)
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <returns>日期部分整数</returns>
        public static int ToDateInt(this DateTime dateTime)
        {
            return dateTime.ToInt();
        }

        /// <summary>
        /// 获取时间部分并转换为整数(HHmmss)
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <returns>时间部分整数</returns>
        public static int ToTimeInt(this DateTime dateTime)
        {
            return dateTime.ToInt("HHmmss");
        }


        /// <summary>
        /// 将整数类型的日期部分和时间部分合并成日期类型，
        /// </summary>
        /// <param name="datePart">日期部分，如 20140313</param>
        /// <param name="timePart">如：91255 或101123</param>
        /// <returns>日期</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        /// int date=20140828;
        /// int time=92359;
        /// 
        /// DateTime dt=DateTimeHelper.ToDateTime(date,time)    //返回 2014-08-28 09:23:59
        /// ]]>
        /// </code>
        /// </example>
        public static DateTime ToDateTime(int datePart, int timePart)
        {
            if (datePart < 19000101) return DateTime.MinValue;
            else if (timePart > 99991231) return DateTime.MaxValue;

            string strDateTime = string.Concat(datePart.ToString("####-##-##"), " ", timePart.ToString().PadLeft(6, '0').Insert(2, ":").Insert(5, ":"));
            return Convert.ToDateTime(strDateTime);
        }

        /// <summary>
        /// 将8位整数转换为日期(yyyyMMdd)
        /// </summary>
        /// <param name="intDate">数字</param>
        /// <param name="dateOut">日期</param>
        /// <returns>true or false</returns>
        public static bool TryToDate(this int intDate, out DateTime dateOut)
        {
            dateOut = new DateTime(1900, 1, 1);

            if (intDate / 10000000 == 0)
            {
                //如果数字长度小于8位，直接返回false;
                return false;
            }

            return intDate.ToString("####-##-##").TryToDate(out dateOut);
        }
        /// <summary>
        /// 转换为日期格式
        /// </summary>
        /// <param name="intDate">数字日期或时间部分</param>
        /// <param name="format">日期格式</param>
        /// <returns>日期</returns>
        public static DateTime ToDateTime(this int intDate, string format = "yyyyMMdd")
        {
#if NETSTANDARD1_6
            IFormatProvider ifp = new CultureInfo("zh-CN");
#else
            IFormatProvider ifp = new CultureInfo("zh-CN", true);
#endif
            return DateTime.ParseExact(intDate.ToString(), format, ifp);
        }


        /// <summary>
        /// 将8位整数转换为日期(yyyyMMdd)
        /// </summary>
        /// <param name="intDate">数字</param>
        /// <returns>日期</returns>
        public static DateTime ToDate(this int intDate)
        {
            try
            {
                return intDate.ToDateTime();
            }
            catch
            {
                return DateTime.MinValue;
            }
        }


        /// <summary>
        /// 获得某年的第一天
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>某年的第一天</returns>
        public static DateTime GetFirstDayOfThisYear(this DateTime date)
        {
            return new DateTime(date.Year, 1, 1);
        }

        /// <summary>
        /// 获得某季度的第一天
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>某季度的第一天</returns>
        public static DateTime GetFirstDayOfThisQuarter(this DateTime date)
        {
            return new DateTime(date.Year, ((date.Month - 1) / 3) * 3 + 1, 1);
        }

        /// <summary>
        /// 获得某月的第一天
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>某月的第一天</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     DateTime dt=new DateTime(2017,5,17);
        ///     
        ///     DateTime firstdt==dt.GetFirstDayOfThisMonth(); // return 2017-05-01
        /// 
        /// ]]>
        /// </code>
        /// </example>
        public static DateTime GetFirstDayOfThisMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// 获得某周的第一天(第一天从星期天开始算)
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>某周的第一天</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     DateTime dt=new DateTime(2017,5,17);
        ///     
        ///     DateTime firstdt==dt.GetFirstDayOfThisWeek(); // return 2017-05-14 //星期天
        /// 
        /// ]]>
        /// </code>
        /// </example>

        public static DateTime GetFirstDayOfThisWeek(this DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Sunday)
                return date.Date;

            return date.Date.AddDays(-(int)date.DayOfWeek);
        }

        /// <summary>
        /// 获取时间 是一年中的第几个星期
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>获取时间 是一年中的第几个星期</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     DateTime dt=new DateTime(2017,5,17);
        ///     
        ///     DateTime firstdt==dt.WeekOfYear(); 
        /// 
        /// ]]>
        /// </code>
        /// </example>
        public static int WeekOfYear(this DateTime date)
        {
            var firstDay = new DateTime(date.Year, 1, 1);
            var days = (date - firstDay).Days + (int)firstDay.DayOfWeek;
            return days / 7 + 1;
        }


        /// <summary>
        /// 将DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="dt">时间</param>
        /// <param name="milliseconds">是否精确到毫秒</param>
        /// <returns> Unix时间戳</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     DateTime dt=new DateTime(2017,5,17);
        ///     
        ///     long  m==dt.ToUnixTime(true);   //精确到毫秒
        ///     
        ///     long s=dt.ToUnixTime();     ///精确到秒
        /// 
        /// ]]>
        /// </code>
        /// </example> 
        public static long ToUnixTime(this DateTime dt, bool milliseconds = false)
        {
            double result;
#if NETSTANDARD1_6
            DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();
#else
            DateTime startTime = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
#endif

            if (milliseconds)
            {
                result = (dt - startTime).TotalMilliseconds;
            }
            else
            {
                result = (dt - startTime).TotalSeconds;
            }
            return (long)result;
        }


        /// <summary>
        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="unixTime">Unix时间戳</param>
        /// <param name="milliseconds">是否精确到毫秒</param>
        /// <returns>返回日期</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     long timespan=1494950400000;
        ///     DateTime dt=timespan.ConvertTimeByUnix(true);
        ///     
        ///     long timespan1=1494950400;
        ///     DateTime dt=timespan1.ConvertTimeByUnix();
        /// 
        /// ]]>
        /// </code>
        /// </example>
        public static DateTime ConvertTimeByUnix(this long unixTime, bool milliseconds = false)
        {
#if NETSTANDARD1_6
            DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();
#else
            DateTime startTime = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
#endif
            if (milliseconds)
            {
                return startTime.AddMilliseconds(unixTime);
            }
            else
            {
                return startTime.AddSeconds(unixTime);
            }
        }


        /// <summary>
        /// 获取两个时间之间的间隔值
        /// </summary>
        /// <param name="startDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="interval">间隔方式</param>       
        /// <returns>间隔值</returns>
        /// <example>
        ///    <code lang="c#">
        /// <![CDATA[
        ///     DateTime dt="2014-08-27 08:00:00";
        ///     DateTime dt1="2014-08-28 08:00:00";
        ///     
        ///     long day=dt1.DateDiff(dt1,DateInterval.Day);    //天数：1
        ///     long hour=dt1.DateDiff(dt1,DateInterval.Hour);  //小时：24
        /// ]]>   
        /// </code>
        /// </example>
        public static long DateDiff(this DateTime endDate, DateTime startDate, DateInterval interval)
        {
            long lngDateDiffValue = 0;
            TimeSpan TS = new TimeSpan(endDate.Ticks - startDate.Ticks);
            switch (interval)
            {
                case DateInterval.Second:
                    lngDateDiffValue = (long)TS.TotalSeconds;
                    break;
                case DateInterval.Minute:
                    lngDateDiffValue = (long)TS.TotalMinutes;
                    break;
                case DateInterval.Hour:
                    lngDateDiffValue = (long)TS.TotalHours;
                    break;
                case DateInterval.Day:
                    lngDateDiffValue = TS.Days;
                    break;
                case DateInterval.Week:
                    lngDateDiffValue = TS.Days / 7;
                    break;
                case DateInterval.Month:
                    lngDateDiffValue = TS.Days / 30;
                    break;
                case DateInterval.Quarter:
                    lngDateDiffValue = (TS.Days / 30) / 3;
                    break;
                case DateInterval.Year:
                    lngDateDiffValue = TS.Days / 365;
                    break;
            }
            return (lngDateDiffValue);
        }

        /// <summary>
        /// 根据日期获取年龄
        /// </summary>
        /// <param name="birthdate"></param>
        /// <returns></returns>
        public static int GetAgeByBirthdate(this DateTime birthdate)
        {
            DateTime now = DateTime.Now;
            int age = now.Year - birthdate.Year;
            if (now.Month < birthdate.Month || (now.Month == birthdate.Month && now.Day < birthdate.Day))
            {
                age--;
            }
            return age < 0 ? 0 : age;
        }
        /// <summary>
        /// 获取1970-01-01至dateTime的毫秒数
        /// </summary>
        public static long GetTimestamp(this DateTime dateTime)
        {

            DateTime dtStart = new DateTime(1970, 1, 1).Add(TimeZoneInfo.Local.BaseUtcOffset);
            return (long)(dateTime - dtStart).TotalMilliseconds;
        }

        /// <summary>
        /// 获取1970-01-01至dateTime的毫秒数
        /// </summary>
        public static long GetSecondsTimestamp(this DateTime dateTime)
        {

            DateTime dtStart = new DateTime(1970, 1, 1).Add(TimeZoneInfo.Local.BaseUtcOffset);
            return (long)(dateTime - dtStart).TotalSeconds;
        }
    }

    /// <summary>
    /// 日期间隔
    /// </summary>
    public enum DateInterval
    {
        /// <summary>
        /// 天数
        /// </summary>
        Day,
        /// <summary>
        /// 小时
        /// </summary>
        Hour,
        /// <summary>
        /// 分钟
        /// </summary>
        Minute,
        /// <summary>
        /// 月
        /// </summary>
        Month,
        /// <summary>
        /// 季度
        /// </summary>
        Quarter,
        /// <summary>
        /// 秒
        /// </summary>
        Second,
        /// <summary>
        /// 周
        /// </summary>
        Week,
        /// <summary>
        /// 年
        /// </summary>
        Year
    }
}
