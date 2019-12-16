using Badmati.BinarySpace.Services.IAppService;
using Badmati.BinarySpace.Services.IRefresh;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badmati.BinarySpace.Services.Refresh
{
    public class BackRefreshService : IBackRefreshService
    {
        
        private readonly ILogger _logger;

        public BackRefreshService(
            ILogger<BackRefreshService> logger
            )
        {
            _logger = logger;
        }
        public async Task StartAsync()
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "");
            _logger.LogInformation($"开始后台刷新任务缓存,任务编号{guid}.");
            //todo refreshservice
            _logger.LogInformation($"任务编号{guid}执行完毕");
        }
    }
}
