using System;

namespace BMT.Server.Infrastructure
{
    /// <summary>
    /// 基础数据转换类
    /// </summary>
    public static class BasicTypeConverterHelper
    {
        #region 尝试将对象转换为指定的类型 转换失败则返回默认值
        /// <summary>
        /// 尝试将对象转换为指定的类型 转换失败则返回默认值
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="obj">待转换对象</param>
        /// <param name="defaultValue">转换失败时返回的默认值</param>
        /// <returns>转换的目标对象</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     string str="123";
        ///     int ret=str.ToType<int>(0); //返回int:123
        ///     str="abc";
        ///     ret=str.ToType<int>(0);     //返回int:0
        /// ]]>
        /// </code>
        /// </example>
        public static T ToType<T>(this object obj, T defaultValue)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return defaultValue;
            }
            bool isIConvertible = obj is IConvertible;
            if (!isIConvertible)
            {
                try
                {
                    return obj.Json2Str().Str2Json<T>();
                }
                catch
                {
                    return defaultValue;
                }
            }
            var type = typeof(T);
            try
            {
                return (T)Convert.ChangeType(obj, type);
            }
            catch
            {
#if NETSTANDARD1_6
                Type[] genericArgumentsType = type.GetGenericArguments();
                if (obj.GetType() != type && (genericArgumentsType != null && genericArgumentsType.Length > 0) && type.GetGenericTypeDefinition() == typeof(Nullable<>))
#else
                if (obj.GetType() != type && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
#endif
                {
                    var valType = type.GetGenericArguments()[0];
                    try
                    {
                        return (T)Convert.ChangeType(obj, valType);
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// 尝试将对象转换为指定的类型 转换失败则返回default(T)
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="obj">待转换对象</param>
        /// <returns>转换的目标对象</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     string str="123";
        ///     int ret=str.ToType<int>(); //返回int:123
        ///     str="abc";
        ///     ret=str.ToType<int>();     //返回int:0
        /// ]]>
        /// </code>
        /// </example>
        public static T ToType<T>(this object obj)
        {
            return ToType<T>(obj, default(T));
        }
        #endregion

        #region 将字符串转换为整数(int)

        /// <summary>
        /// 将字符串转换为数字(int)
        /// </summary>
        /// <param name="str">待转换字符</param>
        /// <param name="result">转换结果</param>
        ///  <param name="defaultval">转换失败时，返回默认值</param>
        /// <returns>转换是否成功</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     string str="abc";
        ///     int ret;
        ///     bool successed=str.TryToInt32(out ret); //false,ret=0;
        ///     
        ///     str="123";
        ///    successed=str.TryToInt32(out ret);   //true,ret=123;
        /// ]]>
        /// </code>
        /// </example>
        public static bool TryToInt32(this string str, out int result, int defaultval = 0)
        {
            result = defaultval;
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            if (Int32.TryParse(str, out result))
            {
                return true;
            }
            result = defaultval;
            return false;
        }

        /// <summary>
        /// 将字符串转换为数字(int)
        /// </summary>
        /// <param name="str">待转换字符</param>
        ///  <param name="defaultval">转换失败时，返回默认值</param>
        /// <returns>返回转换后的值</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     string str="123";
        ///     int ret=str.ToInt32(0); //return 123
        ///     
        ///     str="abc";
        ///     ret=abc.ToInt32(); //return 0;
        /// ]]>
        /// </code>
        /// </example>
        public static int ToInt32(this string str, int defaultval = 0)
        {
            int result;
            str.TryToInt32(out result, defaultval);
            return result;
        }
        #endregion

        #region 将字符串分割成int数组
        /// <summary>
        /// 将字符串分割成int数组
        /// </summary>
        /// <param name="str">待转换字符</param>
        /// <param name="splitChar">分隔符</param>
        /// <returns>返回整数数组</returns>
        /// <example>
        ///<code lange="c#">
        ///<![CDATA[
        /// string str="1,2,a,4";
        /// int[] ret=str.ToIntArray(); //return:[1,2,0,4];
        /// ]]>
        /// </code>
        /// </example>
        public static int[] ToIntArray(this string str, string splitChar = ",")
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(splitChar))
            {
                return new int[0];
            }
            string[] strArray = str.Split(new string[] { splitChar }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray.Length == 0)
            {
                return new int[0];
            }

            return
#if NETSTANDARD1_6
            ArrayHelper
#else
            Array
#endif
           .ConvertAll(strArray, s => s.ToInt32());
        }
        #endregion

        #region 将字符串分割成int数组
        /// <summary>
        /// 将字符串分割成long数组
        /// </summary>
        /// <param name="str">待转换字符</param>
        /// <param name="splitChar">分隔符</param>
        /// <returns>返回长整数数组</returns>
        /// <example>
        ///<code lange="c#">
        ///<![CDATA[
        /// string str="1,2,a,4";
        /// long[] ret=str.ToInt64Array(); //return:[1,2,0,4];
        /// ]]>
        /// </code>
        /// </example>
        public static long[] ToInt64Array(this string str, string splitChar = ",")
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(splitChar))
            {
                return new long[0];
            }
            string[] strArray = str.Split(new string[] { splitChar }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray.Length == 0)
            {
                return new long[0];
            }

            return
#if NETSTANDARD1_6
            ArrayHelper
#else
            Array
#endif
            .ConvertAll(strArray, s => s.ToInt64());
        }

        #endregion

        #region 将字符串分割成字符串数组
        /// <summary>
        /// 将字符串分割成字符串数组
        /// </summary>
        /// <param name="str">待转换字符</param>
        /// <param name="splitChar">分隔符</param>
        /// <returns>返回字符串数组</returns>
        /// <example>
        ///<code lange="c#">
        ///<![CDATA[
        /// string str="1,2,a,4";
        /// string[] ret=str.ToStringArray(); //return:["1","2","a","4"];
        /// ]]>
        /// </code>
        /// </example>
        public static string[] ToStringArray(this string str, string splitChar = ",")
        {
            return str.Split(new string[] { splitChar }, StringSplitOptions.RemoveEmptyEntries);
        }
        #endregion

        #region 将传入数组转换为string类型数组
        /// <summary>
        /// 将传入数组转换为string类型数组
        /// </summary>
        /// <typeparam name="TInput">待转换值类型</typeparam>
        /// <param name="inputArray">待转换数组</param>
        /// <returns>转换后的字符型数组</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     int[] ary=new int[3]{1,2,3};
        ///     string[] ret=BasicConverter(ary);   //return :["1","2","3"]
        /// 
        /// ]]>
        /// </code>
        /// </example>
        public static string[] ToStringArray<TInput>(TInput[] inputArray) where TInput : struct
        {
            if (inputArray == null)
            {
                return null;
            }

            return
#if NETSTANDARD1_6
            ArrayHelper
#else
            Array
#endif
            .ConvertAll(inputArray, delegate (TInput input)
            {
                return input.ToString();
            });
        }
        #endregion

        #region 将字符串转换为长整数(Int64)

        /// <summary>
        /// 将字符串转换为数字(Int64)
        /// </summary>
        /// <param name="str">待转换字符</param>
        /// <param name="result">转换结果</param>
        ///  <param name="defaultval">转换失败时，返回默认值</param>
        /// <returns>转换是否成功</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     string str="123456789012";
        ///     long ret;
        ///     bool successed=str.TryToInt64(out ret); //return:123456789012
        /// ]]>
        /// </code>
        /// </example>
        public static bool TryToInt64(this string str, out long result, long defaultval = 0)
        {
            result = defaultval;
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            if (Int64.TryParse(str, out result))
            {
                return true;
            }
            result = defaultval;
            return false;
        }

        /// <summary>
        /// 将字符串转换为数字(Long)
        /// </summary>
        /// <param name="str">待转换字符</param>
        ///  <param name="defaultval">转换失败时，返回默认值</param>
        /// <returns>返回转换后的值</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        /// string str="123456789012";
        /// long ret=str.ToInt64();// 返回：123456789012
        /// ]]>
        /// </code>
        /// </example>
        public static long ToInt64(this string str, long defaultval = 0)
        {
            long result;
            str.TryToInt64(out result, defaultval);
            return result;
        }
        #endregion

        #region 字符串转换为日期
        /// <summary>
        /// 字符串转换为日期
        /// </summary>
        /// <param name="str">待转换字符</param>
        /// <param name="result">转换结果</param>
        /// <param name="format">日期格式</param>
        /// <returns>是否转换成功,若转换失败，out日期为1900-01-01</returns>
        /// <example>
        /// <code lang="c#">   
        /// <![CDATA[
        /// string str="2104-08-28";
        /// DateTime dt;
        /// bool successed=str.TryToDate(out dt);
        /// 
        /// str="20140828081559";
        /// successed=str.TryToDate(out dt,"yyyyMMddHHmmss");    //dt:2014-08-28 08:15:59
        /// 
        /// str="123";
        /// successed=str.TryToDate(out dt); //false, dt:1900-01-01
        /// ]]>
        /// </code>
        /// </example>
        public static bool TryToDate(this string str, out DateTime result, string format = "yyyy-MM-dd")
        {
            result = DateTime.MinValue;
            if (string.IsNullOrEmpty(format))
            {
                format = "yyyy-MM-dd";
            }
            if (string.IsNullOrEmpty(str) || str.Length != format.Length)
            {
                return false;
            }

            bool flag = DateTime.TryParseExact(str, format,
                System.Globalization.DateTimeFormatInfo.InvariantInfo,
                System.Globalization.DateTimeStyles.None, out result);
            if (!flag)
            {
                result = new DateTime(1900, 1, 1);
            }
            return flag;
        }

        /// <summary>
        ///  字符串转换为日期
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="format">日期格式</param>
        /// <returns>日期</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     DateTime dt="2017-05-17".ToDateTime();
        ///     
        ///     DateTime dt="20170517".ToDateTime("yyyyMMdd");
        /// ]]>
        /// </code>
        /// </example>
        public static DateTime ToDateTime(this string str, string format = "yyyy-MM-dd")
        {
            DateTime result;
            if (str.TryToDate(out result, format))
            {
                return result;
            }
            return result;
        }
        #endregion

        #region 将传入值转换为bool类型
        /// <summary>
        /// 将传入值转换为bool类型
        /// </summary>
        /// <param name="input">待转换值</param>
        /// <param name="defValue">默认值，默认返回false</param>
        /// <returns>转换后的值</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     string str="1";
        ///     bool ret=str.ToBool();  //TRUE;
        ///     
        ///     str="true";
        ///     ret=str.ToBool();       //TRUE;
        ///     
        ///     str="0";
        ///     ret=str.ToBool();       //False;
        ///     
        ///     str="false";
        ///     ret=str.ToBool();       //False;
        /// ]]>
        /// </code>
        /// </example>
        public static bool ToBool(this string input, bool defValue = false)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return defValue;
            }
            bool result = defValue;
            string strInput = input.ToLower();
            if (strInput == "1" || strInput == "true")
            {
                result = true;
            }
            else if (strInput == "0" || strInput == "false")
            {
                result = false;
            }

            return result;
        }
        #endregion

        /// <summary>
        /// 将传入值转换为int类型
        /// </summary>
        /// <param name="input">待转换值</param>
        /// <param name="defValue">默认值</param>
        /// <returns>转换后的值</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     string str="123.1";
        ///     float ret=str.ToFloat();
        ///     
        ///     int i=123;
        ///     ret=i.ToFloat();
        /// ]]>
        /// </code>
        /// </example>
        public static int ToInt32(this object input, int defValue = 0)
        {
            if (input == null)
            {
                return defValue;
            }
            return input.ToType<int>(defValue);
        }

        /// <summary>
        /// 将传入值转换为int类型
        /// </summary>
        /// <param name="input">待转换值</param>
        /// <param name="defValue">默认值</param>
        /// <returns>转换后的值</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     string str="123.1";
        ///     float ret=str.ToFloat();
        ///     
        ///     int i=123;
        ///     ret=i.ToFloat();
        /// ]]>
        /// </code>
        /// </example>
        public static long ToInt64(this object input, long defValue = 0)
        {
            if (input == null)
            {
                return defValue;
            }
            return input.ToType<long>(defValue);
        }

        #region 将传入值转换为float类型
        /// <summary>
        /// 将传入值转换为float类型
        /// </summary>
        /// <param name="input">待转换值</param>
        /// <param name="defValue">默认值</param>
        /// <returns>转换后的值</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     string str="123.1";
        ///     float ret=str.ToFloat();
        ///     
        ///     int i=123;
        ///     ret=i.ToFloat();
        /// ]]>
        /// </code>
        /// </example>
        public static float ToFloat(this object input, float defValue = 0f)
        {
            if (input == null)
            {
                return defValue;
            }
            return input.ToType<float>(defValue);
        }
        #endregion

        #region  将传入值转换为double类型
        /// <summary>
        /// 将传入值转换为double类型
        /// </summary>
        /// <param name="input">待转换值</param>
        /// <param name="defValue">默认值</param>
        /// <returns>转换后的值</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     string str="123.1";
        ///     float ret=str.ToDouble();
        ///     
        ///     int i=123;
        ///     ret=i.ToDouble();
        /// ]]>
        /// </code>
        /// </example>
        public static double ToDouble(this object input, double defValue = 0.0)
        {
            if (input == null)
            {
                return defValue;
            }
            return input.ToType<double>(defValue);
        }
        #endregion

        #region 将传入值转换为decimal类型
        /// <summary>
        /// 将传入值转换为decimal类型
        /// </summary>
        /// <param name="input">待转换值</param>
        /// <param name="defValue">默认值</param>
        /// <returns>转换后的值</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     string str="123.1";
        ///     float ret=str.ToDecimal();
        ///     
        ///     int i=123;
        ///     ret=i.ToDecimal();
        /// ]]>
        /// </code>
        /// </example>
        public static decimal ToDecimal(this object input, decimal defValue = 0M)
        {
            if (input == null)
            {
                return defValue;
            }
            return input.ToType<decimal>(defValue);
        }
        #endregion

    }
     
}
