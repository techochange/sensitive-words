using BMT.Server.Services.IRefresh;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMT.Server.WebApi.TaskJob.Job
{
    [DisallowConcurrentExecution]
    public class RefreshBookJob : IJob
    {
        private readonly IBackRefreshService _refreshBookService;

        public RefreshBookJob(IBackRefreshService refreshBookService)
        {
            _refreshBookService = refreshBookService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            //await Console.Out.WriteLineAsync("RefreshBookJob Start");
            await _refreshBookService.StartAsync();
            //await Console.Out.WriteLineAsync("RefreshBookJob Stop");
        }
    }
}
