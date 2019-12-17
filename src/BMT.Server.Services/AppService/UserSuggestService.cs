using BMT.Server.Infrastructure;
using BMT.Server.Infrastructure.OrmDapper;
using BMT.Server.Services.IAppService;
using BMT.Server.Services.Models;
using BMT.Server.Services.Models.Dtos;
using BMT.Server.Services.Models.Entities;
using BMT.Server.Services.Models.Param;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMT.Server.Services.AppService
{
    public class UserSuggestService : IUserSuggestService
    {
        private readonly IDapperRepository<UserSuggestEntity> _userSuggestRepository;
        public UserSuggestService(
             IDapperRepository<UserSuggestEntity> userSuggestRepository
            )
        {
            _userSuggestRepository = userSuggestRepository;
        }
        public async Task<bool> AddSuggestAsync(ParamUserSuggest param)
        {
            var entity = new UserSuggestEntity
            {
                AppType = SystemConfig.BMT_APP_ID,
                Content = param.Content,
                CreateTime = DateTime.Now,
                Email = param.Email,
                EmailReply = false,
                IsReply = false,
                PhoneNum = param.PhoneNum,
                PhoneReply = false,
                ReplyContent = string.Empty,
                ReplyTime = null,
                State = 0,
                UserId = param.UserId
            };
            return await _userSuggestRepository.InsertAsync(entity);
        }

        public async Task<ListData<UserSuggestDto>> GetUserSuggest(PagedParam param)
        {
            var result = new ListData<UserSuggestDto>();
            result.Total = await _userSuggestRepository.GetCountAsync(new { UserId = param.UserId });
            var data = await _userSuggestRepository.GetPageListAsync(param.PageIndex, param.PageSize, " where UserId = @UserId and AppType=@AppType ", " createTime desc ", new { UserId = param.UserId,AppType = SystemConfig.BMT_APP_ID });
            result.List = (from a in data
                          select new UserSuggestDto(a)).ToList();
            return result;
        }
    }
}
