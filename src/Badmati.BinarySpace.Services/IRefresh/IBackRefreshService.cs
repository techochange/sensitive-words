using Badmati.BinarySpace.Infrastructure.AutoFac;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Badmati.BinarySpace.Services.IRefresh
{
    public interface IBackRefreshService:ISingleton
    {
        Task StartAsync();
    }
}
