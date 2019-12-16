using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Com.Ctrip.Framework.Apollo;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Badmati.BinarySpace.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logFactory = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config");
            var logger = logFactory.GetCurrentClassLogger();
            try
            {
                #if DEBUG 
                //阿波罗配置系统日志，如果需要查看日志打开这段代码
                //Com.Ctrip.Framework.Apollo.Logging.LogManager.Provider = new ApolloProviderLogger();
                #endif
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stop program");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }

        }

        /// <summary>
        /// 引入配置apollo程序，使用nlog记录日志
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
            //阿波罗配置文件，如果不需要可以注释
                //.ConfigureAppConfiguration(option => {
                //    option.AddJsonFile("appsettings.json", true, true);
                //    var iRoot = option.Build().GetSection("apollo");
                //    option.AddApollo(iRoot)
                //    .AddDefault();
                //})
                
                .UseKestrel()
                //.UseUrls("http://127.0.0.1:4567")
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog()
            ;
    } 
}
