using Badmati.BinarySpace.Infrastructure;
using Badmati.BinarySpace.Services.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Badmati.BinarySpace.WebApi.Controllers
{
    public class BaseController:Controller
    {
        protected readonly ILogger _logger;
        
        private readonly IHostingEnvironment _hostingEnvironment;
        public BaseController()
        {
        }

        public BaseController(IHostingEnvironment hostingEnviroment, ILogger logger)
        {
            this._hostingEnvironment = hostingEnviroment;
            this._logger = logger;
        }
        
        /// <summary>
        /// 取根目录
        /// </summary>
        protected string RootPath
        {
            get
            {
                return _hostingEnvironment.WebRootPath??_hostingEnvironment.ContentRootPath;
            }
        }

        protected virtual bool ValidParam(PrmModel param)
        {
            if (param == null || (string.IsNullOrEmpty(param.UserId)&&string.IsNullOrEmpty(param.DeviceId)))
            {

                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 验证失败
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Func">The function.</param>
        /// <returns></returns>
        protected async Task<RpsModel<bool>> ValidFailAsync(PrmModel param)
        {
            var rpsModel = new RpsModel<bool>();
            try
            {
                rpsModel.Status = SystemConfig.APP_RESULT_CODE_BASE + (int)HttpStatusCode.BadRequest;
                rpsModel.Data = false;
                rpsModel.IsSuccess = false;
                rpsModel.Msg = "缺省参数";

                rpsModel.TraceId = GuidHelper.NewOne();
                _logger.LogWarning(rpsModel.TraceId + param.Json2Str());
                
            }
            catch (Exception ex)
            {
                rpsModel.Status = SystemConfig.APP_RESULT_CODE_BASE + (int)HttpStatusCode.InternalServerError;
                rpsModel.Msg = ex.Message;
                rpsModel.IsSuccess = false;
                
            }
            return rpsModel;
        }

        /// <summary>
        /// 返回单条结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Func">The function.</param>
        /// <returns></returns>
        protected async Task<RpsModel<T>> OperationSingleAsync<T>(Func<RpsModel<T>, Task<T>> Func)
        {
            var rpsModel = new RpsModel<T>();
            try
            {
                rpsModel.Status = SystemConfig.APP_RESULT_CODE_BASE + (int)HttpStatusCode.OK;
                rpsModel.Data = await Func(rpsModel);
                //rpsModel.Msg = string.Empty;
            }
            catch (Exception ex)
            {
                rpsModel.Status = SystemConfig.APP_RESULT_CODE_BASE + (int)HttpStatusCode.InternalServerError;
                rpsModel.Msg = ex.Message;
                rpsModel.IsSuccess = false;
                rpsModel.TraceId = GuidHelper.NewOne();
                _logger.LogError(rpsModel.TraceId + ex.Message + ex.StackTrace, ex);
            }
            return rpsModel;
        }


        /// <summary>
        /// 返回多条结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Func">The function.</param>
        /// <returns></returns>
        protected async Task<RpsListModel<T>> OperationMultipleAsync<T>(Func<RpsListModel<T>, Task<ListData<T>>> Func)
        {
            var rpsModel = new RpsListModel<T>();
            try
            {
                rpsModel.Status = SystemConfig.APP_RESULT_CODE_BASE + (int)HttpStatusCode.OK;
                var listData = await Func(rpsModel);
                rpsModel.List = listData.List;
                rpsModel.Total = listData.Total;
                //rpsModel.Msg = string.Empty;
                rpsModel.IsSuccess = true;
            }
            catch (Exception ex)
            {
                rpsModel.Status = SystemConfig.APP_RESULT_CODE_BASE + (int)HttpStatusCode.InternalServerError;
                rpsModel.Msg = ex.Message;
                rpsModel.IsSuccess = false;
                rpsModel.TraceId = GuidHelper.NewOne();
                _logger.LogError(rpsModel.TraceId + ex.Message + ex.StackTrace, ex);
            }
            return rpsModel;
        }
    }
}
