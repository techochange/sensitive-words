using BMT.Server.Infrastructure.AutoFac;
using BMT.Server.Services.Models;
using BMT.Server.Services.Models.Dtos;
using BMT.Server.Services.Models.Param;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BMT.Server.Services.IAppService
{
    public interface IUserSuggestService:IScoped
    {
        Task<bool> AddSuggestAsync(ParamUserSuggest param);


        Task<ListData<UserSuggestDto>> GetUserSuggest(PagedParam param);
    }
}
