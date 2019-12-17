using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMT.Server.Infrastructure.Quartz
{
    public class QuartzJonFactory : IJobFactory
    {
        private readonly IServiceProvider serviceProvider;

        private readonly ILogger logger;

        public QuartzJonFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger(typeof(QuartzJonFactory));
            this.serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            IJob job = serviceProvider.GetServices<IJob>().FirstOrDefault(o => o.GetType() == bundle.JobDetail.JobType);
            if (job == null)
            {
                logger.LogWarning($"{bundle.JobDetail.Key} 任务不存在");
            }
            return job;
        }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}
