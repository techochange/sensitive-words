using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Badmati.BinarySpace.Infrastructure;
using Badmati.BinarySpace.Services.IAppService;
using Badmati.BinarySpace.Services.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Badmati.BinarySpace.WebApi.Controllers
{
    [Route("api/file/[action]")]
    public class FileController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public FileController(ILogger<FileController> logger,
           IHostingEnvironment hostingEnvironment):base(hostingEnvironment,logger)
        {
            this._hostingEnvironment = hostingEnvironment;
        }
        [HttpPost]
        public async Task<RpsBaseModel> UploadImg()
        {
            return await OperationSingleAsync<string>(async x =>
            {

                var data = HttpContext.Request.Form;

                var files = HttpContext.Request.Form.Files;
                //var licNO = string.Empty;
                //if (!HttpContext.Request.Form.TryGetValue("licNO", out var licNO))
                //{
                var licNO = Guid.NewGuid().ToString().Replace("-", "");
                //}
                var file = files.FirstOrDefault();
                if (file != null)
                {
                    var extName = "."+file.ContentType.Split('/')[1];
                    //var extName = file.FileName.Substring(file.FileName.LastIndexOf("."));
                    var filePath = RootPath+ "/licImg/" + licNO+ extName;

                    if (file.Length > 0)
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                            return licNO + extName;
                        }
                    }
                }
                return "";

            });
        }

        [HttpGet]
        public async Task<RpsBaseModel> GetSingleData()
        {
            return await OperationMultipleAsync<bool>(async x =>
            {
                return new ListData<bool> { Total = 5, List = new List<bool> { true,false,false,true,true } };
            });
        }

        [HttpGet]
        public async Task<RpsBaseModel> GetListData()
        {
            return await OperationSingleAsync<object>(async x =>
            {
                return new {  id =1,text="我是测试君"};
            });
        }
    }
}
