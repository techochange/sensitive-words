using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Badmati.BinarySpace.Infrastructure.Quartz
{
    public static class QuartzHostedServiceExtensions
    {
        public static IServiceCollection AddQuartzHostedService(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<IJobFactory, QuartzJonFactory>();
            return serviceCollection;
        }
    }
}
