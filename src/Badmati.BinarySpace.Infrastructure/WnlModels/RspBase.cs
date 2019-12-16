using System;
using System.Collections.Generic;
using System.Text;

namespace Badmati.BinarySpace.Infrastructure.BadmatiModels
{
    public class RspBase : IRspBase
    {
        /// <summary>
        /// 请求响应状态
        /// 200正常 304 无修改 400 参数错误 500 服务器异常
        /// </summary>
        public int Status { get; set; } = 200;
        /// <summary>
        /// 服务器下发标识当前内容的sign标识，
        /// 如果服务器下发了这个值，客户端请求时需要上传
        /// </summary>
        //public string Sign { get; set; } = string.Empty;
        /// <summary>
        /// 服务器返回消息字段
        /// </summary>
        public string Msg { get; set; } = string.Empty;
    }

    /// <summary>
    /// 基础业务返回类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RspBase<T> : RspBase
    {
        /// <summary>
        /// 具体业务数据字段
        /// </summary>
        public T Data { get; set; }
    }

    /// <summary>
    /// 基础分页业务返回类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RspPageBase<T> : RspBase<T>
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public long Total { get; set; }
    }
}
