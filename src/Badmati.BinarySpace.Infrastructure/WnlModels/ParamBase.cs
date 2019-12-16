using System;
using System.Collections.Generic;
using System.Text;

namespace Badmati.BinarySpace.Infrastructure.BadmatiModels
{
    /// <summary>
    /// 基础请求参数类
    /// </summary>
    public class ParamBase : IParamBase
    {
        public virtual bool IsParamValidate()
        {
            return true;
        }
        
    }

    /// <summary>
    /// 分页查询基础类
    /// </summary>
    public class ParamPageBase : ParamBase
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
