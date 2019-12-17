using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BMT.Server.Services.IAppService;

namespace BMT.Server.WebApi.HostService
{
    public class CacheRefreshService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;

        public CacheRefreshService(ILogger<CacheRefreshService> logger )//, ITaskAppService service)
        {
            this._logger = logger;            
        }

        private void DoWork(object state)
        {
            //BMTRedisHelper
            this._logger.LogInformation("Time Background Service is working.");
        }

        public void Dispose()
        {
            this._timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //每5s执行一个任务
            this._timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this._timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
