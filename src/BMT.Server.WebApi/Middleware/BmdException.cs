using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StackExchange.Exceptional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMT.Server.Infrastructure;
using BMT.Server.Infrastructure.BMTModels;
using BMT.Server.Services.AppExceptions;

namespace BMT.Server.WebApi.Middleware
{ 
    /// <summary>
    /// 统一的异常处理中间件
    /// 将异常通过 exceptional 记录到统一的管理后台方便查看
    /// </summary>
    public class BMTException
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<BMTException> _logger;
        public BMTException(RequestDelegate next, ILogger<BMTException> logger)
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
        public static IApplicationBuilder UseBMTExpMiddleware(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            return app.UseMiddleware<BMTException>();
        }
    }
}
