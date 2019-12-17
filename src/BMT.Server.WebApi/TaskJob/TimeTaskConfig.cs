using BMT.Server.Infrastructure.Quartz;
using BMT.Server.WebApi.TaskJob.Job;
using BMT.Server.WebApi.TaskJob.JobHandler;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMT.Server.WebApi.TaskJob
{
    public static class TimeTaskConfig
    {
        public static IServiceCollection InitTask(this IServiceCollection services)
        {

            services.AddSingleton<IJobHandler, RefreshBookJobHandler>();

            services.AddSingleton(typeof(IJob), typeof(RefreshBookJob));
            return services;
        }
    }
}
