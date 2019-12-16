using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StackExchange.Exceptional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Badmati.BinarySpace.Infrastructure;
using Badmati.BinarySpace.Infrastructure.BadmatiModels;
using Badmati.BinarySpace.Services.AppExceptions;

namespace Badmati.BinarySpace.WebApi.Middleware
{ 
    /// <summary>
    /// 统一的异常处理中间件
    /// 将异常通过 exceptional 记录到统一的管理后台方便查看
    /// </summary>
    public class BadmatiException
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<BadmatiException> _logger;
        public BadmatiException(RequestDelegate next, ILogger<BadmatiException> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            RspBase rspBase = new RspBase();
            try
            {
                await this._next(context);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message, context.Request.Query);
                context.Response.ContentType = "application/json; charset=utf-8";                
                if (ex is BaseAppExcetion)
                {
                    rspBase.Msg = ex.Message;
                    rspBase.Status = 500;
                    await context.Response.WriteAsync(rspBase.Json2Str());
                }
                else
                {
                    await ex.LogAsync(context);
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync(ex.Json2Str());
                }
            }
        }
    }

    public static class ExceptionMiddlewareExtension
    {
        public static IApplicationBuilder UseBadmatiExpMiddleware(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            return app.UseMiddleware<BadmatiException>();
        }
    }
}
