using System;
using System.Collections.Generic;
using System.Text;

namespace Badmati.BinarySpace.Infrastructure
{
    public class SystemConfig
    {
        public const int APP_RESULT_CODE_BASE = 100000;
        /// <summary>
        /// 佰码蹄应用id
        /// </summary>
        public const int BADMATI_APP_ID = APP_RESULT_CODE_BASE;

        public const string APP_PRIFIX = "quickRead_";

        public static string DB_CONNECTION_STRTING = string.Empty;

        /// <summary>
        /// 四大名著Id
        /// </summary>
        public static string FOUR_BOOKIDS = "33,45,57,74";

        /// <summary>
        /// 首页推荐列表书
        /// </summary>
        public static int SUGGEST_COUNT = 10;

        /// <summary>
        /// APP 启动后延迟5分钟开始刷新缓存
        /// </summary>
        public static int APP_REFRESH_DELAY_MINUTE = 1;

        /// <summary>
        /// 每隔120分钟，刷新一次缓存
        /// </summary>
        public static int APP_REFRESH_INTERVAL_MINUTE = 120;


        //public static string SUGGEST_BOOK_IDS = "";

        /// <summary>
        /// APP tab 页编号 A001 热门
        /// </summary>
        public const string APP_TAB_TYPE_A001 = "A001";
        /// <summary>
        /// APP tab 页编号 A002 经典
        /// </summary>
        public const string APP_TAB_TYPE_A002 = "A002";
        /// <summary>
        /// APP tab 页编号 A003 诗词
        /// </summary>
        public const string APP_TAB_TYPE_A003 = "A003";
        /// <summary>
        /// APP tab 页编号 A004 小说
        /// </summary>
        public const string APP_TAB_TYPE_A004 = "A004";
        public const string APP_TAB_TYPE_A005 = "A005";
        public const string APP_TAB_TYPE_A006 = "A006";
    }
}
