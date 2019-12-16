using System;
using System.Collections.Generic;
using System.Text;

namespace Badmati.BinarySpace.Infrastructure
{
    /// <summary>
    /// 身份证验证帮助类
    /// </summary>
    public abstract class VerifyIDCard
    {
        /// <summary>
        /// 省份验证字符串
        /// </summary>
        const string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

        public string IDCard { get; private set; }

        protected VerifyIDCard(string idCard)
        {
            IDCard = idCard;
        }
        /// <summary>
        /// 是否通过验证
        /// </summary>
        /// <returns></returns>
        protected virtual bool Verify()
        {
            if (VerifyNumber() == false)
            {
                return false;
            }
            if (VerifyProvince() == false)
            {
                return false;
            }
            if (VerifyBrithDay() == false)
            {
                return false;
            }
            return VerifyBit();
        }
        /// <summary>
        /// 验证身份证是否合法
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public static bool IsVerified(string idCard)
        {
            int len = idCard.Length;
            if (len == 18)
            {
                return new VerifyIDCard18(idCard).Verify();
            }
            return false;
        }


        /// <summary>
        /// 数字验证
        /// </summary>
        /// <returns></returns>
        protected abstract bool VerifyNumber();

        /// <summary>
        /// 验证省份
        /// </summary>
        /// <returns></returns>
        private bool VerifyProvince()
        {
            string provinceCode = IDCard.Remove(2);
            return address.Contains(provinceCode);
        }

        /// <summary>
        /// 验证生日
        /// </summary>
        /// <returns></returns>
        protected abstract bool VerifyBrithDay();

        /// <summary>
        /// 验证校验位
        /// </summary>
        /// <returns></returns>
        protected abstract bool VerifyBit();

    }

    #region 18位身份证号验证
    /// <summary>
    /// 18位身份证验证
    /// </summary>
    public class VerifyIDCard18 : VerifyIDCard
    {

        public VerifyIDCard18(string idCard)
            : base(idCard)
        {
        }

        protected override bool VerifyNumber()
        {
            long n;
            if (long.TryParse(IDCard.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(IDCard.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            return true;
        }

        protected override bool VerifyBrithDay()
        {
            string birth = IDCard.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            return true;
        }

        protected override bool VerifyBit()
        {
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] ai = IDCard.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(wi[i]) * int.Parse(ai[i].ToString());
            }
            int y = -1;

#if NETSTANDARD1_6
                y = sum % 11;
#else
            Math.DivRem(sum, 11, out y);
#endif
            if (arrVarifyCode[y] != IDCard.Substring(17, 1).ToLower())
            {
                return false;//验证码验证
            }
            return true;
        }
    }
    #endregion
}
