using Badmati.BinarySpace.Infrastructure.AutoFac;
using Badmati.BinarySpace.Services.Models;
using Badmati.BinarySpace.Services.Models.Dtos;
using Badmati.BinarySpace.Services.Models.Param;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Badmati.BinarySpace.Services.IAppService
{
    public interface IUserSuggestService:IScoped
    {
        Task<bool> AddSuggestAsync(ParamUserSuggest param);


        Task<ListData<UserSuggestDto>> GetUserSuggest(PagedParam param);
    }
}
