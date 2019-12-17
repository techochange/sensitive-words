using BMT.Server.Infrastructure.AutoFac;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BMT.Server.Services.IAppService
{
    public interface IBlackWordsService:IScoped
    {
        Task<bool> IsIncludeBlackWords(string content,string rootPath);

        Task<List<string>> LoadBlackWordsDictionary(string rootPath, bool refresh=false);
    }
}
