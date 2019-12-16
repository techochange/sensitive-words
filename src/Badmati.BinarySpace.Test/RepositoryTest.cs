using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Badmati.BinarySpace.Infrastructure.OrmDapper;
using Badmati.BinarySpace.Services.Models.Task;
using Badmati.BinarySpace.WebApi;

namespace Badmati.BinarySpace.Test
{
    [TestClass]
    public class RepositoryTest
    {
        private readonly IDapperRepository<EntityTask> _repository;

        public RepositoryTest()
        {
            var client = new TestServer(WebHost.CreateDefaultBuilder().UseStartup<Startup>());
            _repository = client.Host.Services.GetService(typeof(BaseDapperRepository<EntityTask>)) as IDapperRepository<EntityTask>;
        }

        [TestMethod]
        public void GetList()
        {
            
            var result = this._repository.GetPageListAsync(1, 10, string.Empty, string.Empty).GetAwaiter().GetResult();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
        }
    }
}
