using BMT.Server.Services.IAppService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BMT.Server.Infrastructure;

namespace BMT.Server.Services.AppService
{
    public class BlackWordsService : IBlackWordsService
    {
        public BlackWordsService()
        {
        }
        private List<string> GetBlackWordsFromFile(string rootPath)
        {
            var list = new List<string>();
            var path = rootPath +"\\" + "resource";
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    var contents = FileHelper.ReadFile(file);
                    if (!string.IsNullOrEmpty(contents))
                    {
                        list.AddRange(contents.Split("\r\n"));
                    }
                }
            }
            return list;
        }
        public async Task<bool> IsIncludeBlackWords(string content,string rootPath)
        {
            var list = await LoadBlackWordsDictionary(rootPath);
            var result = false;
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (string.IsNullOrEmpty(item.Trim()))
                    {
                        continue;
                    }
                    if (content.Contains(item))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        public async Task<List<string>> LoadBlackWordsDictionary(string rootPath, bool refresh = false)
        {
            List<string> blackWordsList = new List<string>();
            if (refresh)
            {
                blackWordsList = GetBlackWordsFromFile(rootPath);
                if (blackWordsList.Count > 0)
                {
                    MemoryCacheTime.SetChacheValue(ConstDefine.BLACK_WORDS_CACHE_KEY, blackWordsList, ConstDefine.QUICKAPP_CACHE_FIFTEEN_MINUTES);
                }
            }
            blackWordsList = (List<string>)MemoryCacheTime.GetCacheValue(ConstDefine.BLACK_WORDS_CACHE_KEY);
            if (blackWordsList== null || blackWordsList.Count == 0)
            {
                blackWordsList = GetBlackWordsFromFile(rootPath);
                if (blackWordsList != null)
                {
                    MemoryCacheTime.SetChacheValue(ConstDefine.BLACK_WORDS_CACHE_KEY, blackWordsList, ConstDefine.QUICKAPP_CACHE_FIFTEEN_MINUTES);
                }
            }
            return blackWordsList;
        }
    }
}
