using Badmati.BinarySpace.Infrastructure.Quartz;
using Badmati.BinarySpace.WebApi.TaskJob.Job;
using Badmati.BinarySpace.WebApi.TaskJob.JobHandler;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badmati.BinarySpace.WebApi.TaskJob
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
