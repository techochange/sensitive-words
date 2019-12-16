using System;
using System.Collections.Generic;
using System.Text;

namespace Badmati.BinarySpace.Infrastructure
{
    public static class RandomHelper
    {
        public static readonly Random Random = new Random();
        public const string SourceCode = "2YU9IP6ASDFG8QWERTHJ7KLZX4CV5B3ONM1";
        public const string DefaultSourceCode = "NBGHS";

        public static string GetRandomCode(int length = 6)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                int randNumber = Random.Next(0, 9);
                sb.Append(randNumber);
            }

            return sb.ToString();
        }

        public static string GetUniqueInviteCode(int Id)
        {
            string code = "";
            int mod = 0;
            StringBuilder sb = new StringBuilder();
            while (Id > 0)
            {
                mod = Id % SourceCode.Length;
                Id = (Id - mod) / SourceCode.Length;
                code = SourceCode.ToCharArray()[mod] + code;

            }
            return code.PadRight(6, '0');//不足六位补0
        }

        public static int DecodeUniqueInviteCode(string code)
        {

            code = code.Replace("0", "");
            int num = 0;
            for (int i = 0; i < code.ToCharArray().Length; i++)
            {
                for (int j = 0; j < SourceCode.ToCharArray().Length; j++)
                {
                    if (code.ToCharArray()[i] == SourceCode.ToCharArray()[j])
                    {
                        num += j * Convert.ToInt32(Math.Pow(SourceCode.Length, code.ToCharArray().Length - i - 1));
                    }
                }
            }
            return num;
        }

    }
}
