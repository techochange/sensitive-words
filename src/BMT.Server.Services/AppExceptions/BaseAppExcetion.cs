using System;
using System.Collections.Generic;
using System.Text;

namespace BMT.Server.Services.AppExceptions
{
    /// <summary>
    /// 应用错误
    /// 所有应用自定义错误需要继承这个类
    /// </summary>
    public class BaseAppExcetion : Exception
    {
        public BaseAppExcetion(string msg) : base(msg)
        {

        }

        public BaseAppExcetion(string msg,Exception ex) : base(msg,ex)
        {

        }
    }
}
