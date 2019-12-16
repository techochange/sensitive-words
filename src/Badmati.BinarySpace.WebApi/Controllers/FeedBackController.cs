using Badmati.BinarySpace.Services.IAppService;
using Badmati.BinarySpace.Services.Models;
using Badmati.BinarySpace.Services.Models.Dtos;
using Badmati.BinarySpace.Services.Models.Param;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badmati.BinarySpace.WebApi.Controllers
{
    [Route("api/[Controller]/[action]")]
    public class FeedBackController: BaseController
    {
        private IUserSuggestService _userSuggestService;
        public FeedBackController(ILogger<FeedBackController> logger
           ,IUserSuggestService userSuggestService
           ,IHostingEnvironment hostingEnvironment) : base(hostingEnvironment, logger)
        {
            _userSuggestService = userSuggestService;
        }
        [HttpPost]
        public async Task<RpsBaseModel> AddSuggest([FromBody]ParamUserSuggest param)
        {
            return await OperationSingleAsync<bool>(async x =>
            {
                return await _userSuggestService.AddSuggestAsync(param);
            });
        }


        [HttpGet]
        public async Task<RpsBaseModel> GetSuggest([FromQuery]PagedParam param)
        {
            return await OperationMultipleAsync<UserSuggestDto>(async x =>
            {
                return await _userSuggestService.GetUserSuggest(param);
            });
        }
    }
}
