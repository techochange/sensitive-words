using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Badmati.BinarySpace.Infrastructure
{
    public static class BasicValidator
    {
        #region 字符串验证

        /// <summary>
        /// 检查是否为空字符串,并移除首尾空字符
        /// </summary>
        /// <param name="str">待检测字符</param>
        /// <returns>true-是空字符串，false:非空字符串</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        /// string str="";
        /// bool ret=str.IsNullOrEmptyAndTrim();    //返回 true
        /// 
        /// str="   ";
        /// ret=str.IsNullOrEmptyAndTrim();    //返回 true
        /// ]]>
        /// </code>
        /// </example>
        public static bool IsNullOrEmptyAndTrim(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return true;
            str = str.Trim();
            if (string.IsNullOrEmpty(str))
                return true;
            return false;
        }

        /// <summary>
        /// 正则表达式匹配
        /// </summary>
        /// <param name="str">待检查字符串</param>
        /// <param name="pattern">表达式</param>
        /// <returns>是否匹配成功</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        /// 
        /// string str="123";
        /// string pattern="^\\d+$";
        /// 
        /// bool isMatch=str.IsMatch(pattern);  //返回 True
        /// 
        /// ]]>
        /// </code>
        /// </example>
        public static bool IsMatch(this string str, string pattern)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            if (string.IsNullOrEmpty(pattern))
                throw new ArgumentNullException("pattern");
            return Regex.IsMatch(str, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 忽略大小写匹配是否包含
        /// </summary>
        /// <param name="str">待检查字符串</param>
        /// <param name="value">待检测包含的字符</param>
        /// <returns>是否匹配成功</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        /// 
        /// string str="abcDEF";
        /// string str1="cd";
        /// bool isContain=str.ContainsIgnoreCase(str1);    //返回True
        /// 
        /// ]]>
        /// </code>
        /// </example>
        public static bool ContainsIgnoreCase(this string str, string value)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");
            if (str.Length < value.Length)
                return false;
            return str.ToLower().Contains(value.ToLower());
        }

        #region 身份证号验证
        /// <summary>
        /// 判断身份证号码是否合法，默认严格验证，需要验证身份证号的校验位
        /// </summary>
        /// <param name="inputCard">身份证号码</param>
        /// <returns>true or false</returns>
        ///<example>
        ///<code lang="c#">
        ///<![CDATA[
        /// string idcard="123456789987654321"  //len:18
        /// bool isIDCard=idCard.IsIDCard();    //False
        /// 
        /// idcard="330719196804253671";    //真实身份证号
        /// isIDCard=idCard.IsIDCard();    //True
        /// ]]>
        /// </code>
        /// </example>
        public static bool IsIDCard(this string inputCard)
        {
            return VerifyIDCard.IsVerified(inputCard);
        }

        #endregion

        /// <summary>
        /// 是否为手机号，支持 13x,14x,15x,17x,18x
        /// </summary>
        /// <param name="mobile">待验证字符串</param>
        /// <returns>是否验证通过</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     string mobile="13813813812";
        ///     bool isMobile=mobile.IsMobile();    //True
        /// ]]>
        /// </code>
        /// </example>
        public static bool IsMobile(this string mobile)
        {
            return Regex.IsMatch(mobile, @"^1([3|4|5|7|8][0-9])\d{8}$");
        }

        /// <summary>
        /// 是否为有效电话号码，支持格式8位和7位无区号电话号码和有区号电话号码
        /// 如：86711234,057186711234,0571-86711234
        /// </summary>
        /// <param name="str">待验证字符串</param>
        /// <returns>是否验证通过</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     string phone="86711234";
        ///     bool ret=phone.IsPhone();   //True;
        ///     
        ///     phone="057186711234"
        ///     ret=phone.IsPhone();   //True;
        ///     
        ///     phone="0571-86711234"
        ///     ret=phone.IsPhone();   //True;
        /// ]]>
        /// </code>
        /// </example>
        public static bool IsPhone(this string str)
        {
            if (string.IsNullOrEmpty(str) || str.Length < 7 || str.Length > 13)
            {
                return false;
            }
            bool isMatched = Regex.IsMatch(str, @"(0[0-9]{2,3}\-)?([2-9][0-9]{6,7})+(\-[0-9]{1,4})?$");
            if (isMatched)
            {
                if (!str.Contains("-") && str.Length > 12)
                {
                    return false;
                }
            }
            return isMatched;
        }

        /// <summary>
        /// 是否为ip字符串
        /// </summary>
        /// <param name="ip">ip字符串</param>
        /// <returns>是否验证通过</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     
        ///     string ip="218.218.218.218";
        ///     bool ret=ip.IsIP(); //True
        /// ]]>
        /// </code>
        /// </example>
        public static bool IsIP(this string ip)
        {
            string pattrn = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
            return Regex.IsMatch(ip, pattrn);
        }

        /// <summary>
        /// 是否为合法的Email地址
        /// </summary>
        /// <param name="email">待验证Email</param>
        /// <returns>是否验证通过</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     string email="test@g.cn";
        ///     bool ret=email.IsEmail();   //True
        /// ]]>
        /// </code>
        /// </example>
        public static bool IsEmail(this string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// 判断是否为整数字符串集合
        /// 格式：1,2,3,4,5,6
        /// </summary>
        /// <param name="ids">待验证字符串</param>
        /// <returns>是否验证通过</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        /// string str="1,2,3,4,5,6";
        /// bool ret=str.IsNumberArray();   //True
        /// ]]>
        /// </code>
        /// </example>
        public static bool IsNumberArray(this string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return false;
            }
            return Regex.IsMatch(ids, @"^([0-9]+([,][0-9]+)*)$");
        }

        /// <summary>
        /// 是否安全(是否存在SQL注入) 存在返回true 不存在返回false<br/>
        /// <i>建议所有SQL语句操作均使用参数化查询，避免SQL注入</i>
        /// </summary>
        /// <param name="str">待检测字符串</param>
        /// <returns>如果存在SQL注入则返回True，否则返回Flase</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        ///     string sql="id=1--xp_cmdshell";
        ///     bool ret=sql.IsUnSafeSql(); //True:存在SQL注入风险
        /// ]]>
        /// </code>
        /// </example>
        public static bool IsUnSafeSql(this string str)
        {
            return Regex.IsMatch(str, @"[-|;|,|\/|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 是否为正确的Url(http/https/ftp)
        /// </summary>
        /// <param name="strUrl">要验证的Url</param>
        /// <returns>判断结果</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        /// string str="http://www.g.cn";
        /// bool ret=str.IsUrl();       //True
        /// 
        /// str="https://www.g.cn"      
        /// ret=str.IsUrl();       //True
        /// 
        /// str="ftp://www.g.cn"      
        /// ret=str.IsUrl();       //True
        /// ]]>
        /// </code>
        /// </example>
        public static bool IsUrl(this string strUrl)
        {
            return Regex.IsMatch(strUrl, @"^(http|https|ftp)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }

        /// <summary>
        /// 是否是否为日期格式
        /// </summary>
        /// <param name="str">待验证字符串</param>
        /// <returns>是否验证通过</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        /// string str="2014-08-28";
        /// 
        /// bool ret=str.IsDateTime();      //True
        /// ]]>
        /// </code>
        /// </example>
        public static bool IsDateTime(this string str)
        {
            DateTime dt;
            return DateTime.TryParse(str, out dt);
        }

        /// <summary>
        /// 是否都为中文
        /// </summary>
        /// <param name="str">待验证字符串</param>
        /// <returns>是否验证通过</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        /// string str="杭州";
        /// bool ret=str.IsChinese();   //True
        /// 
        /// str="hangzhou杭州";
        /// ret=str.IsChinese();   //False
        /// ]]>
        /// </code>
        /// </example>
        public static bool IsChinese(this string str)
        {
            return Regex.IsMatch(str, "^[\u4e00-\u9fa5]*$");
        }

        /// <summary>
        ///  验证传入值是否Int32整数
        /// </summary>
        /// <param name="input">待验证值</param>
        /// <returns>是否为Int32整数</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        /// string str="+123";
        /// bool ret=str.IsInt32(); //True
        /// 
        /// str="-123";
        ///  ret=str.IsInt32(); //True
        /// ]]>
        /// </code>
        /// </example>
        public static bool IsInt32(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            int result = 0;
            return int.TryParse(input, out result);
        }

        /// <summary>
        ///  验证传入值是否Int64长整数
        /// </summary>
        /// <param name="input">待验证值</param>
        /// <returns>是否为Int64长整数</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        /// string str="+123";
        /// bool ret=str.IsInt64(); //True
        /// 
        /// str="-123";
        ///  ret=str.IsInt64(); //True
        /// ]]>
        /// </code>
        /// </example>
        public static bool IsInt64(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            long result = 0;
            return long.TryParse(input, out result);
        }

        /// <summary>
        /// 是否为有效的整数
        /// </summary>
        /// <param name="str">待验证字符串</param>
        /// <returns>是否为有效的整数</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        /// string str="+123";
        /// bool ret=str.IsInteger(); //True
        /// 
        /// str="-123";
        ///  ret=str.IsInteger(); //True
        /// ]]>
        /// </code>
        /// </example>
        public static bool IsInteger(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            long val;
            return long.TryParse(str, out val);
        }

        /// <summary>
        ///  验证传入值是否float类型值
        /// </summary> 
        /// <param name="input">待验证值</param>
        /// <returns>是否为float类型值</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        /// string str="+123.1";
        /// bool ret=str.IsFloat(); //True
        /// 
        /// str="-123.1";
        ///  ret=str.IsFloat(); //True
        /// ]]>
        /// </code>
        /// </example>
        public static bool IsFloat(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            float result = 0;
            return float.TryParse(input, out result);
        }

        /// <summary>
        ///  验证传入值是否double类型值
        /// </summary>
        /// <param name="input">待验证值</param>
        /// <returns>是否为double类型值</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        /// string str="+123.1";
        /// bool ret=str.IsDouble(); //True
        /// 
        /// str="-123.1";
        ///  ret=str.IsDouble(); //True
        /// ]]>
        /// </code>
        /// </example>
        public static bool IsDouble(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            double result = 0;
            return double.TryParse(input, out result);
        }

        /// <summary>
        ///  验证传入值是否decimal类型值
        /// </summary>
        /// <param name="input">待验证值</param>
        /// <returns>是否为decimal类型值</returns>
        /// <example>
        /// <code lang="c#">
        /// <![CDATA[
        /// string str="+123.1";
        /// bool ret=str.IsDecimal(); //True
        /// 
        /// str="-123.1";
        ///  ret=str.IsDecimal(); //True
        /// ]]>
        /// </code>
        /// </example>
        public static bool IsDecimal(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            decimal result = 0;
            return decimal.TryParse(input, out result);
        }

        /// <summary>
        /// 检测是否为有效的MAC地址
        /// </summary>
        /// <param name="mac">MAC地址</param>
        /// <returns>true是mac地址,false-不是</returns>
        ///<example>
        ///<code lang="c#">
        ///<![CDATA[
        /// string str="00-50-56-C0-00-08"
        /// bool ret=str.IsMac();   //True
        /// ]]>
        /// </code>
        /// </example>
        public static bool IsMac(this string mac)
        {
            if (string.IsNullOrEmpty(mac))
                return false;
            return Regex.IsMatch(mac, @"^([0-9a-fA-F]{2})(([/\s:-][0-9a-fA-F]{2}){5})$");
        }

        #endregion
    }
}
