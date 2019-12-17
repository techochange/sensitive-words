using Microsoft.AspNetCore.Mvc.Filters;
using StackExchange.Exceptional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMT.Server.Infrastructure.BMTModels;
using BMT.Server.Services.AppExceptions;
using System.IO;

namespace BMT.Server.WebApi.Filters
{
    public class BmdExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            WriteLog(context.Exception.Message + context.Exception.StackTrace+context.Exception.InnerException);
            if (context.Exception is BaseAppExcetion)
            { 
                RspBase rspBase = new RspBase();
                context.HttpContext.Response.ContentType = "application/json; charset=utf-8";
                rspBase.Msg = context.Exception.Message;
                rspBase.Status = 500;
                context.HttpContext.Response.StatusCode = 200;
                context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(rspBase);                
            }
            else
            {
                context.Exception.LogAsync(context.HttpContext);
                context.HttpContext.Response.StatusCode = 500;
                //await context.Response.WriteAsync(ex.Json2Str()); 
            }
            context.ExceptionHandled = true;
        }

        private void WriteLog(string text)
        {

            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = System.IO.Path.Combine(path
            , "Logs");

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            string fileFullName = System.IO.Path.Combine(path+Path.DirectorySeparatorChar
            , string.Format("{0}.txt", DateTime.Now.ToString("yyyyMMdd-HHmm")));


            using (StreamWriter output = System.IO.File.AppendText(fileFullName))
            {
                output.WriteLine(text);

                output.Close();
            }
        }
    }
}
