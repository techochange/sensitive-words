using System;
using System.Collections.Generic;
using System.Text;

namespace Badmati.BinarySpace.Services
{
    public static class ConstDefine
    {
        #region 微信
        /// <summary>
        /// 小程序 app_id
        /// </summary>
        public const string APP_ID = "wx07e0f79b49824a89";//"wx28e12e9e33209058";
        /// <summary>
        /// 小程序密钥
        /// </summary>
        public const string APP_SECURET = "faac200b0b7c63c62b04a29191cd67e0";//"ed069abdd538eed32b88683d7ae2d74b";

        /// <summary>
        /// 商户号
        /// </summary>
        public const string MCH_ID = "1554882681";
        /// <summary>
        /// 交易类型
        /// </summary>
        public const string TRADE_TYPE = "JSAPI";

        public const string POS_ID = "5201314";

        /// <summary>
        /// 交易密钥
        /// </summary>
        public const string TRADE_KEY = "ABCabc1234ABCabc1234ABCabc1234AB";

        /// <summary>
        /// 小程序接入地址
        /// </summary>
        public const string WECHAT_ACCESS_TOKEN_URL = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + APP_ID + "&secret=" + APP_SECURET;

        /// <summary>
        /// 令牌过期时间，默认时2小时
        /// </summary>
        public const int EXPIRED_IN = 7200;

        /// <summary>
        /// js_code 取访问令牌
        /// </summary>
        public const string WECHAT_JSCODE_TO_SESSION_URL = "https://api.weixin.qq.com/sns/jscode2session?appid="+APP_ID+"&secret="+APP_SECURET+"&js_code={0}&grant_type=authorization_code";
        #endregion


        #region 快应用 经典诗词小说

        public const string QUICKAPP_PREFIX = "quickApp_";

        /// <summary>
        /// 书籍缓存关键字
        /// </summary>
        public const string QUICKAPP_BOOK_CACHE_KEY = QUICKAPP_PREFIX + "book";

        /// <summary>
        /// 轮播图缓存关键字
        /// </summary>
        public const string QUICKAPP_SLIDESHOW_CACHE_KEY = QUICKAPP_PREFIX + "slideShow";

        /// <summary>
        /// 轮播图缓存关键字
        /// </summary>
        public const string QUICKAPP_SLIDE_TEXT_CACHE_KEY = QUICKAPP_PREFIX + "slideText";

        /// <summary>
        /// 四大名著缓存关键字
        /// </summary>
        public const string QUICKAPP_BOOK_FOUR_CACHE_KEY = QUICKAPP_BOOK_CACHE_KEY + "four";
        /// <summary>
        /// 章节缓存关键字
        /// </summary>
        public const string QUICKAPP_CHAPTER_CACHE_KEY = QUICKAPP_PREFIX + "chapter";
        /// <summary>
        /// 段落缓存关键字
        /// </summary>
        public const string QUICKAPP_PARAGRAPH_CACHE_KEY = QUICKAPP_PREFIX + "paragraph";

        /// <summary>
        /// 分类缓存关键字
        /// </summary>
        public const string QUICKAPP_CLASSTYPE_CACHE_KEY = QUICKAPP_PREFIX + "classType";

        /// <summary>
        /// 首页推荐关键字
        /// </summary>
        public const string QUICKAPP_SUGGEST_BOOK_CACHE_KEY = QUICKAPP_PREFIX + "suggestBook";

        /// <summary>
        /// 分类书籍缓存关键字
        /// </summary>
        public const string QUICKAPP_CLASS_BOOK_CACHE_KEY = QUICKAPP_PREFIX + "classBook";

        /// <summary>
        /// 每日小喇叭缓存关键字
        /// </summary>
        public const string QUICKAPP_DAILYWORDS_CACHE_KEY = QUICKAPP_PREFIX + "dailyWords";

        /// <summary>
        /// H5内容
        /// </summary>
        public const string QUICKAPP_H5CONTENT_CACHE_KEY = QUICKAPP_PREFIX + "h5content";

        /// <summary>
        /// 缓存绝对过期时间设置为1000
        /// </summary>
        public const int QUICKAPP_CACHE_EXPIRE_TIME = 3600 * 24 * 1000;//默认缓存1000天

        /// <summary>
        /// 缓存2小时
        /// </summary>
        public const int QUICKAPP_CACHE_TWO_HOURS = 7200;
        /// <summary>
        /// 缓存1天
        /// </summary>
        public const int QUICKAPP_CACHE_ONE_DAY = 3600 * 24;
        /// <summary>
        /// 15分钟内无访问时释放资源
        /// </summary>
        public const int QUICKAPP_CACHE_DISPOSE = 900;
        #endregion
    }
}
