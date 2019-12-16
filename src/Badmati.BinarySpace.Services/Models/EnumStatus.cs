using System;
using System.Collections.Generic;
using System.Text;

namespace Badmati.BinarySpace.Services.Models
{
    public enum EnumStatus
    {
        禁用=0,
        启用=1
    }
    public enum EnumWechatCode
    {
        /// <summary>
        /// 系统繁忙
        /// </summary>
        SERVER_ERROR = -1,
        /// <summary>
        /// 请求成功
        /// </summary>
        SUCCESS = 0,
        /// <summary>
        /// Code无效
        /// </summary>
        CODE_ERROR = 40029,
        /// <summary>
        /// 频率限制,1分钟100次
        /// </summary>
        RATE_LIMIT = 45011
    }

    public enum EnumGender
    {
        保密=0,
        男=1,
        女=2
    }

    /// <summary>
    /// 我的-我喜欢的 左侧图标枚举
    /// </summary>
    public enum EnumDailyWordsIconInx
    {
        /// <summary>
        /// 今天
        /// </summary>
        Today,
        /// <summary>
        /// 昨天
        /// </summary>
        Yesterday,
        /// <summary>
        /// 更早的第一天
        /// </summary>
        TheDaysBeforeYesterday,
        /// <summary>
        /// 更早
        /// </summary>
        More=3
    }

    public class GenderTextHelper
    {
        public static int GetGenderText(string gender)
        {
            switch (gender)
            {
                case "男":return (int)EnumGender.男;
                case "女":return (int)EnumGender.女;
                default:return (int)EnumGender.保密;
            }
        }
    }

    /// <summary>
    /// 链接动作类型
    /// </summary>
    public enum EnumLinkType
    {
        /// <summary>
        /// 什么也不做
        /// </summary>
        无动作,
        /// <summary>
        /// 阅读页或者文案明细页
        /// </summary>
        进入明细页,
        /// <summary>
        /// h5页
        /// </summary>
        跳转到指定页,
        /// <summary>
        /// Tab 页
        /// </summary>
        跳转到指定Tab
    }

    /// <summary>
    /// tab页数字编号
    /// </summary>
    public enum EnumTabNumber
    {
        A001,
        A002,
        A003,
        A004,
        A005,
        A006
    }

    public enum EnumH5ContentType
    {
        文字,
        图片
    }
}
