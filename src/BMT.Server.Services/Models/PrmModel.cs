using System;
using System.Collections.Generic;
using System.Text;

namespace BMT.Server.Services.Models
{
    public class PrmModel
    {
        /// <summary>
        /// 系统版本
        /// </summary>
        public string Ver { get; set; }
        /// <summary>
        /// 渠道商 OPPO vivo XIAOMI HUAWEI 魅族 等
        /// </summary>
        public string Chl { get; set; }
        /// <summary>
        /// 设备型号
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// 设备品牌
        /// </summary>
        public string Brd { get; set; }
        /// <summary>
        /// 操作系统类别
        /// </summary>
        public int OsType { get; set; }
        /// <summary>
        /// 操作系统版本名称
        /// </summary>
        public string OsVerName { get; set; }
        /// <summary>
        /// 操作系统版本号
        /// </summary>
        public string OsVerCode { get; set; }
        /// <summary>
        /// 系统区域
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// 运行平台名称
        /// </summary>
        public string PlatformVersionName { get; set; }
        /// <summary>
        /// 运行平台版本号
        /// </summary>
        public int PlatformVersionCode { get; set; }
        /// <summary>
        /// 广告唯一id
        /// </summary>
        public string Advertising { get; set; }
        /// <summary>
        /// 设备Id
        /// </summary>
        public string DeviceId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        public DateTime? Day { get; set; }

        /// <summary>
        /// pageNumber 页码默认1
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// pageSize，页大小 默认20
        /// </summary>
        public int PageSize { get; set; } = 50;

        #region 以下几个参数为小米2019年新起草规范
        /// <summary>
        /// 匿名设备标志符
        /// </summary>
        public string OAID { get; set; }
        /// <summary>
        /// 设备唯一标识符
        /// </summary>
        public string UDID { get; set; }
        /// <summary>
        /// 开发者匿名标志符
        /// </summary>
        public string VAID { get; set; }
        /// <summary>
        /// 应用匿名标志符
        /// </summary>
        public string AAID { get; set; }
        #endregion
    }

    public class PagedParam : PrmModel
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        
    }
    public class PagedParam<T> : PagedParam
    {
        public T Condition { get; set; }
    }
}
