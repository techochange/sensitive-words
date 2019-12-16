using System;
using System.Collections.Generic;
using System.Text;

namespace Badmati.BinarySpace.Infrastructure
{

    /**
     * 随机数字生成器，基本原理：
     * 1）入参ID：1 
     * 2）使用自定义进制转换之后为：6 
     * 3）转换未字符串，并在后面添加'5'：75 
     * 4）在79后面再随机补足4位，得到：7586132661 
     * 5）反向转换时以'5'为分界线，'5'后面的不再解析
     *
     * @author 陈太启
     */
    class CodeHelper10
    {

        /**
         * 自定义9进制，数组顺序可进行调整增加反推难度，5用来补位因此此数组不包含5，共9个字符。
         */
        private static char[] BASE = new char[] { '6', '8', '0', '1', '4', '3', '9', '2', '7' };

        /**
         * 5补位字符，不能与自定义重复
         */
        private static char SUFFIX_CHAR = '5';

        /**
         * 进制长度
         */
        private static int BIN_LEN = BASE.Length;

        /**
         * 生成数字序列
         */
        private static int CODE_LEN = 10;

        /**
         * ID转换为邀请码
         *
         * @param id
         * @return
         */
        public static string IdToCode(long id)
        {
            char[] buf = new char[BIN_LEN];
            int charPos = BIN_LEN;

            // 当id除以数组长度结果大于0，则进行取模操作，并以取模的值作为数组的坐标获得对应的字符
            while (id / BIN_LEN > 0)
            {
                int index = (int)(id % BIN_LEN);
                buf[--charPos] = BASE[index];
                id /= BIN_LEN;
            }

            buf[--charPos] = BASE[(int)(id % BIN_LEN)];
            // 将字符数组转化为字符串
            string result = new string(buf, charPos, BIN_LEN - charPos);

            // 长度不足指定长度则随机补全
            int len = result.Length;
            if (len < CODE_LEN)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(SUFFIX_CHAR);
                Random random = new Random();
                // 去除SUFFIX_CHAR本身占位之后需要补齐的位数
                for (int i = 0; i < CODE_LEN - len - 1; i++)
                {
                    sb.Append(BASE[random.Next(BIN_LEN)]);
                }

                result += sb.ToString();
            }
            return result;
        }
    }


    class CodeHelper12
    {

        /**
         * 自定义15进制，数组顺序可进行调整增加反推难度，X用来补位因此此数组不包含X，共15个字符。
         */
        private static char[] BASE = new char[] { 'I', '7', '6', 'L', '5', 'E', 'O', '2', 'V','4','W','8','Z','N','Y','3','U','K' };

        /**
         * X补位字符，不能与自定义重复
         */
        private static char SUFFIX_CHAR = 'X';

        /**
         * 进制长度
         */
        private static int BIN_LEN = BASE.Length;

        /**
         * 生成数字序列
         */
        private static int CODE_LEN = 12;

        /**
         * ID转换为邀请码
         *
         * @param id
         * @return
         */
        public static string IdToCode(long id)
        {
            char[] buf = new char[BIN_LEN];
            int charPos = BIN_LEN;

            // 当id除以数组长度结果大于0，则进行取模操作，并以取模的值作为数组的坐标获得对应的字符
            while (id / BIN_LEN > 0)
            {
                int index = (int)(id % BIN_LEN);
                buf[--charPos] = BASE[index];
                id /= BIN_LEN;
            }

            buf[--charPos] = BASE[(int)(id % BIN_LEN)];
            // 将字符数组转化为字符串
            string result = new string(buf, charPos, BIN_LEN - charPos);

            // 长度不足指定长度则随机补全
            int len = result.Length;
            if (len < CODE_LEN)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(SUFFIX_CHAR);
                Random random = new Random();
                // 去除SUFFIX_CHAR本身占位之后需要补齐的位数
                for (int i = 0; i < CODE_LEN - len - 1; i++)
                {
                    sb.Append(BASE[random.Next(BIN_LEN)]);
                }

                result += sb.ToString();
            }
            return result;
        }

        public static int CodeToId(string code)
        {
            try
            {
                char[] charArray = code.ToCharArray();
                int result = 0;
                for (int i = 0; i < charArray.Length; i++)
                {
                    int index = 0;
                    for (int j = 0; j < BIN_LEN; j++)
                    {
                        if (charArray[i] == BASE[j])
                        {
                            index = j;
                            break;
                        }
                    }

                    if (charArray[i] == SUFFIX_CHAR)
                    {
                        break;
                    }

                    if (i > 0)
                    {
                        result = result * BIN_LEN + index;
                    }
                    else
                    {
                        result = index;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return 0;
                //throw;
            }
        }
    }
    class CodeHelper6
    {

        /**
         * 自定义9进制，数组顺序可进行调整增加反推难度，5用来补位因此此数组不包含5，共9个字符。
         */
        private static char[] BASE = new char[] { '3', '9', '5', '4', '8', '1', '7', '0', '6' };

        /**
         * 5补位字符，不能与自定义重复
         */
        private static char SUFFIX_CHAR = '2';

        /**
         * 进制长度
         */
        private static int BIN_LEN = BASE.Length;

        /**
         * 生成数字序列
         */
        private static int CODE_LEN = 6;

        /**
         * ID转换为邀请码
         *
         * @param id
         * @return
         */
        public static string IdToCode(long id)
        {
            char[] buf = new char[BIN_LEN];
            int charPos = BIN_LEN;

            // 当id除以数组长度结果大于0，则进行取模操作，并以取模的值作为数组的坐标获得对应的字符
            while (id / BIN_LEN > 0)
            {
                int index = (int)(id % BIN_LEN);
                buf[--charPos] = BASE[index];
                id /= BIN_LEN;
            }

            buf[--charPos] = BASE[(int)(id % BIN_LEN)];
            // 将字符数组转化为字符串
            string result = new string(buf, charPos, BIN_LEN - charPos);

            // 长度不足指定长度则随机补全
            int len = result.Length;
            if (len < CODE_LEN)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(SUFFIX_CHAR);
                Random random = new Random();
                // 去除SUFFIX_CHAR本身占位之后需要补齐的位数
                for (int i = 0; i < CODE_LEN - len - 1; i++)
                {
                    sb.Append(BASE[random.Next(BIN_LEN)]);
                }

                result += sb.ToString();
            }
            return result;
        }
    }

    class CodeHelper6Str
    {

        /**
         * 自定义15进制，数组顺序可进行调整增加反推难度，5用来补位因此此数组不包含B，共6个字符。
         */
        private static char[] BASE = new char[] { 'T', 'Y', 'P', 'O', 'V', 'X', 'E', 'Q', 'W','C','A','K','N','S','R' };

        /**
         * 5补位字符，不能与自定义重复
         */
        private static char SUFFIX_CHAR = 'B';

        /**
         * 进制长度
         */
        private static int BIN_LEN = BASE.Length;

        /**
         * 生成数字序列
         */
        private static int CODE_LEN = 6;

        /**
         * ID转换为邀请码
         *
         * @param id
         * @return
         */
        public static string IdToCode(int id)
        {
            char[] buf = new char[BIN_LEN];
            int charPos = BIN_LEN;

            // 当id除以数组长度结果大于0，则进行取模操作，并以取模的值作为数组的坐标获得对应的字符
            while (id / BIN_LEN > 0)
            {
                int index = (int)(id % BIN_LEN);
                buf[--charPos] = BASE[index];
                id /= BIN_LEN;
            }

            buf[--charPos] = BASE[(int)(id % BIN_LEN)];
            // 将字符数组转化为字符串
            string result = new string(buf, charPos, BIN_LEN - charPos);

            // 长度不足指定长度则随机补全
            int len = result.Length;
            if (len < CODE_LEN)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(SUFFIX_CHAR);
                Random random = new Random();
                // 去除SUFFIX_CHAR本身占位之后需要补齐的位数
                for (int i = 0; i < CODE_LEN - len - 1; i++)
                {
                    sb.Append(BASE[random.Next(BIN_LEN)]);
                }

                result += sb.ToString();
            }
            return result;
        }

        public static int CodeToId(string code)
        {
            try
            {
                char[] charArray = code.ToCharArray();
                int result = 0;
                for (int i = 0; i < charArray.Length; i++)
                {
                    int index = 0;
                    for (int j = 0; j < BIN_LEN; j++)
                    {
                        if (charArray[i] == BASE[j])
                        {
                            index = j;
                            break;
                        }
                    }

                    if (charArray[i] == SUFFIX_CHAR)
                    {
                        break;
                    }

                    if (i > 0)
                    {
                        result = result * BIN_LEN + index;
                    }
                    else
                    {
                        result = index;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return 0;
                //throw;
            }
        }
    }

    class CodeHelper8
    {

        /**
         * 自定义9进制，数组顺序可进行调整增加反推难度，7用来补位因此此数组不包含7，共9个字符。
         */
        private static char[] BASE = new char[] { '4', '5', '3', '8', '1', '2', '9', '6', '0' };

        /**
         * 5补位字符，不能与自定义重复
         */
        private static char SUFFIX_CHAR = '7';

        /**
         * 进制长度
         */
        private static int BIN_LEN = BASE.Length;

        /**
         * 生成数字序列
         */
        private static int CODE_LEN = 8;

        /**
         * ID转换为邀请码
         *
         * @param id
         * @return
         */
        public static string IdToCode(long id)
        {
            char[] buf = new char[BIN_LEN];
            int charPos = BIN_LEN;

            // 当id除以数组长度结果大于0，则进行取模操作，并以取模的值作为数组的坐标获得对应的字符
            while (id / BIN_LEN > 0)
            {
                int index = (int)(id % BIN_LEN);
                buf[--charPos] = BASE[index];
                id /= BIN_LEN;
            }

            buf[--charPos] = BASE[(int)(id % BIN_LEN)];
            // 将字符数组转化为字符串
            string result = new string(buf, charPos, BIN_LEN - charPos);

            // 长度不足指定长度则随机补全
            int len = result.Length;
            if (len < CODE_LEN)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(SUFFIX_CHAR);
                Random random = new Random();
                // 去除SUFFIX_CHAR本身占位之后需要补齐的位数
                for (int i = 0; i < CODE_LEN - len - 1; i++)
                {
                    sb.Append(BASE[random.Next(BIN_LEN)]);
                }

                result += sb.ToString();
            }
            return result;
        }
    }

    class CodeHelper8X16
    {

        /**
         * 自定义27进制，数组顺序可进行调整增加反推难度，5用来补位因此此数组不包含2，共27个字符。
         */
        private static char[] BASE = new char[] { 'g', 'a', 'b', 'h', 'e', 'i', 'f', 'j', 'd', 'H', 'i', 'J', 'V', 'F', 'A', 'D', 'C', 'z', 'B', 'X', 'w', 'q', 'n', 'M', 'u', 't', 'p' };

        /**
         * 5补位字符，不能与自定义重复
         */
        private static char SUFFIX_CHAR = '2';

        /**
         * 进制长度
         */
        private static int BIN_LEN = BASE.Length;

        /**
         * 生成数字序列
         */
        private static int CODE_LEN = 8;

        /**
         * ID转换为邀请码
         *
         * @param id
         * @return
         */
        public static string IdToCode(long id)
        {
            char[] buf = new char[BIN_LEN];
            int charPos = BIN_LEN;

            // 当id除以数组长度结果大于0，则进行取模操作，并以取模的值作为数组的坐标获得对应的字符
            while (id / BIN_LEN > 0)
            {
                int index = (int)(id % BIN_LEN);
                buf[--charPos] = BASE[index];
                id /= BIN_LEN;
            }

            buf[--charPos] = BASE[(int)(id % BIN_LEN)];
            // 将字符数组转化为字符串
            string result = new string(buf, charPos, BIN_LEN - charPos);

            // 长度不足指定长度则随机补全
            int len = result.Length;
            if (len < CODE_LEN)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(SUFFIX_CHAR);
                Random random = new Random();
                // 去除SUFFIX_CHAR本身占位之后需要补齐的位数
                for (int i = 0; i < CODE_LEN - len - 1; i++)
                {
                    sb.Append(BASE[random.Next(BIN_LEN)]);
                }

                result += sb.ToString();
            }
            return result;
        }
    }


    public class IDCodeHelper
    {
        /// <summary>
        /// 返回10位伪随机数字
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string IdToCode10(long id)
        {
            return CodeHelper10.IdToCode(id);
        }
        /*
        public static string IdToCode8(long id)
        {
            return CodeHelper8.IdToCode(id);
        }
        */
        /// <summary>
        /// 返回16位伪随机数字
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string IdToCode16(long id)
        {
            return CodeHelper10.IdToCode(id) + CodeHelper6.IdToCode(id);
        }
        /// <summary>
        /// 返回8位数字加8位字母组成的16位字符串
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string IdToCode8X16(long id)
        {
            return CodeHelper8.IdToCode(id) + CodeHelper8X16.IdToCode(id);
        }

        public static string IdToCode6Str(int id)
        {
            return CodeHelper6Str.IdToCode(id);
        }

        public static int CodeStrToId(string code)
        {
            return CodeHelper6Str.CodeToId(code);
        }


        public static string IdToCode12(long id)
        {
            return CodeHelper12.IdToCode(id);
        }

        public static long Code12ToId(string code)
        {
            return CodeHelper12.CodeToId(code);
        }
    }
}
