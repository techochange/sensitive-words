using BMT.Server.Infrastructure.AutoFac;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BMT.Server.Services.IRefresh
{
    public interface IBackRefreshService:ISingleton
    {
        Task StartAsync();
    }
}
