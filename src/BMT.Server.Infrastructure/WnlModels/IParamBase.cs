using System;
using System.Collections.Generic;
using System.Text;

namespace BMT.Server.Infrastructure.BMTModels
{
    /// <summary>
    /// 基础请求参数接口
    /// </summary>
    public interface IParamBase
    {
        /// <summary>
        /// 检查参数是否合法
        /// </summary>
        /// <returns></returns>
        bool IsParamValidate();
    }
}
