using BMT.Server.Services.IAppService;
using BMT.Server.Services.Models;
using BMT.Server.Services.Models.Dtos;
using BMT.Server.Services.Models.Param;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMT.Server.WebApi.Controllers
{
    [Route("api/[Controller]/[action]")]
    public class FeedBackController : BaseController
    {
        private IUserSuggestService _userSuggestService;
        private IBlackWordsService _blackWordsService;
        public FeedBackController(ILogger<FeedBackController> logger
           , IUserSuggestService userSuggestService
           , IBlackWordsService blackWordsService
           , IHostingEnvironment hostingEnvironment) : base(hostingEnvironment, logger)
        {
            _userSuggestService = userSuggestService;
            _blackWordsService = blackWordsService;
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


        [HttpPost]
        public async Task<RpsBaseModel> IncludeBlackWords([FromBody]ParamUserSuggest param)
        {
            return await OperationSingleAsync<bool>(async x =>
            {
                return await _blackWordsService.IsIncludeBlackWords(param.Content, base.AppPath);
            });
        }
    }
}
