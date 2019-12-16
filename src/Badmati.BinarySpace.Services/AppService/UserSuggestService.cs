using Badmati.BinarySpace.Infrastructure;
using Badmati.BinarySpace.Infrastructure.OrmDapper;
using Badmati.BinarySpace.Services.IAppService;
using Badmati.BinarySpace.Services.Models;
using Badmati.BinarySpace.Services.Models.Dtos;
using Badmati.BinarySpace.Services.Models.Entities;
using Badmati.BinarySpace.Services.Models.Param;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badmati.BinarySpace.Services.AppService
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
                AppType = SystemConfig.BADMATI_APP_ID,
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
            var data = await _userSuggestRepository.GetPageListAsync(param.PageIndex, param.PageSize, " where UserId = @UserId and AppType=@AppType ", " createTime desc ", new { UserId = param.UserId,AppType = SystemConfig.BADMATI_APP_ID });
            result.List = (from a in data
                          select new UserSuggestDto(a)).ToList();
            return result;
        }
    }
}
