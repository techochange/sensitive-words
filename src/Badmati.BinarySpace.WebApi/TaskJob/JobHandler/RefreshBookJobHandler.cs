using Badmati.BinarySpace.Infrastructure;
using Badmati.BinarySpace.Infrastructure.Quartz;
using Badmati.BinarySpace.WebApi.TaskJob.Job;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badmati.BinarySpace.WebApi.TaskJob.JobHandler
{
    public class RefreshBookJobHandler : IJobHandler
    {
        public string JobName => "RefreshJob";

        public IJobDetail CreateJobDetail()
        {
            return JobBuilder.Create<RefreshBookJob>().WithIdentity(JobName).Build();
        }

        public ITrigger CreateTigger()
        {
            //延迟2分钟开始
            return TriggerBuilder.Create().WithIdentity(JobName).StartAt(DateTimeOffset.Now.AddMinutes(SystemConfig.APP_REFRESH_DELAY_MINUTE))
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(SystemConfig.APP_REFRESH_INTERVAL_MINUTE)//每间隔2小时，刷新一次所有数据
                    .RepeatForever()//永不间断
                ).Build();
        }
    }
}
