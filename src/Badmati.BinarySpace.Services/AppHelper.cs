using System;
using System.Collections.Generic;
using System.Text;

namespace Badmati.BinarySpace.Services
{
    public class AppHelper
    {
        public static Random _random = new Random();
        public static string GetIconByClassicId(int classicId, string bookName)
        {
            var result = string.Empty;
            switch (bookName)
            {
                case "红楼梦": return "hlm";
                case "三国演义": return "sgyy";
                case "西游记": return "xyj";
                case "水浒传": return "shz";
            }
            switch (classicId)
            {
                default:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    {
                        result = "xs";
                        break;
                    }
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                    {
                        result = "sc";
                        break;
                    }
            }
            //如果书籍长度大于5（一行显示不完），返回2行样式
            result += bookName.Length > 5 ? "2" : "";
            return result;
        }


        public static string GetCssIconId(int classicId, string bookName)
        {
            var result = string.Empty;
            switch (bookName)
            {
                case "红楼梦": return "hlm.png";
                case "三国演义": return "sgyy.png";
                case "西游记": return "xyj.png";
                case "水浒传": return "shz.png";
            }
            switch (classicId)
            {
                default:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    {
                        result = "xs";
                        break;
                    }
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                    {
                        result = "sc";
                        break;
                    }
            }
            //如果书籍长度大于5（一行显示不完），返回2行样式
            result += bookName.Length > 5 ? "2" : "";
            return result;
        }

        public static List<int> GerRandomList(List<int> list, int lenth)
        {
            var result = new List<int>();
            for (var i = 0; i < lenth; i++)
            {
                var data = GetRandomOne(list, 0, lenth);
                list.Add(data);
            }
            return list;
        }

        public static int GetRandomOne(List<int> list, int min, int max)
        {
            var rd = _random.Next(min, max);
            if (list.Contains(rd))
            {
                return GetRandomOne(list, min, max);
            }
            else
            {
                return rd;
            }
        }
    }
}
