using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMT.Server.WebApi.ApolloExtension
{ 
    /// <summary>
    /// 实例化apollo日志类，当需要添加日志时，初始化它
    /// 一般在测试环境开启，线上环境可以不用开启
    /// </summary>
    public class ApolloProviderLogger : Com.Ctrip.Framework.Apollo.Logging.ILoggerProvider
    {
        public ApolloProviderLogger()
        {
        }

        public Com.Ctrip.Framework.Apollo.Logging.ILogger CreateLogger(string name)
        {
            return new ApolloLogger(name);
        }
    }

    public class ApolloLogger : Com.Ctrip.Framework.Apollo.Logging.ILogger
    {
        private readonly NLog.Logger logger;
        public ApolloLogger(string name)
        {
            var logFactory = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config");
            logger = logFactory.GetLogger(name);
        }

        public bool IsEnabled(Com.Ctrip.Framework.Apollo.Logging.LogLevel level)
        {
            return true;
        }

        public void Log(Com.Ctrip.Framework.Apollo.Logging.LogLevel level, string message)
        {
            logger.Info(level.ToString() + message);
        }

        public void Log(Com.Ctrip.Framework.Apollo.Logging.LogLevel level, string message, Exception exception)
        {
            logger.Error(exception, level.ToString() + message);
        }
    }
}
