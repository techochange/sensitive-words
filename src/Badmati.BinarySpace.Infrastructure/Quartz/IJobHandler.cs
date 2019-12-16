using Quartz;
using System;
using System.Collections.Generic;
using System.Text;

namespace Badmati.BinarySpace.Infrastructure.Quartz
{
    public interface IJobHandler
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        string JobName { get; }

        /// <summary>
        /// 创建任务
        /// </summary>
        /// <returns></returns>
        IJobDetail CreateJobDetail();

        /// <summary>
        /// 创建任务触发器
        /// </summary>
        /// <returns></returns>
        ITrigger CreateTigger();
    }
}
