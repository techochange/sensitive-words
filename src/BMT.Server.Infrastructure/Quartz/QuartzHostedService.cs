using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BMT.Server.Infrastructure.Quartz
{
    public class QuartzHostedService : IHostedService
    {
        private readonly ISchedulerFactory schedulerFactory;

        private readonly IServiceProvider serviceProvider;

        private readonly ILogger logger;

        public QuartzHostedService(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger(typeof(QuartzHostedService));
            this.serviceProvider = serviceProvider;
            schedulerFactory = new StdSchedulerFactory();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                try
                {
                    var scheduler = schedulerFactory.GetScheduler(cancellationToken).Result;

                    var jobHandlers = serviceProvider.GetServices<IJobHandler>();
                    if (jobHandlers == null || !jobHandlers.Any())
                    {
                        return;
                    }

                    scheduler.JobFactory = serviceProvider.GetService<IJobFactory>();
                    scheduler.Start(cancellationToken);//启动job
                    foreach (var jobHandler in jobHandlers)
                    {
                        scheduler.ScheduleJob(jobHandler.CreateJobDetail(), jobHandler.CreateTigger(), cancellationToken);
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e, "QuartzHostedService Start error"+e.Message+"666");
                }

            }, cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                try
                {
                    IScheduler scheduler = schedulerFactory.GetScheduler(cancellationToken).Result;
                    var jobKeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup(), cancellationToken).Result;
                    if (jobKeys == null)
                    {
                        return;
                    }
                    foreach (var job in jobKeys)
                    {
                        scheduler.Interrupt(job, cancellationToken);
                    }
                    scheduler.Shutdown(true, cancellationToken).Wait(new TimeSpan(0, 0, 60));
                }
                catch (Exception e)
                {
                    logger.LogError(e, "QuartzHostedService Stop error" + e.Message+"666");
                }

            }, cancellationToken);
        }
    }
}
