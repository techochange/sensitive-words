using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Badmati.BinarySpace.Infrastructure;

namespace Badmati.BinarySpace.Test
{
    [TestClass]
    public class MongoTest
    {  
        public string collectionName = "Peoples";

        [TestMethod]
        public void Insert()
        {
            BadmatiMongodbHelper.Initialization("mongodb://svcsbx:123qwe@192.168.1.110:14789/Badmati_svc_sbx2", "Badmati_svc_sbx2");
           

            BadmatiMongodbHelper.Insert(new MongoEntity { Age = 25, BirthDay = new DateTime(1988, 7, 20), Name = "Bob" }, collectionName);

            var list = BadmatiMongodbHelper.GetAll<MongoEntity>(collectionName);
            Assert.IsTrue(list != null && list.Count > 0);
        }

        [TestMethod]
        public void Query()
        {
            BadmatiMongodbHelper.Initialization("mongodb://svcsbx:123qwe@192.168.1.110:14789/Badmati_svc_sbx2", "Badmati_svc_sbx2");

            var list = BadmatiMongodbHelper.Query<MongoEntity>((a) => a.Name == "Bob" && a.Age == 25, collectionName);

            Assert.IsTrue(list.Count == 1);

            list = BadmatiMongodbHelper.Query<MongoEntity>(a => a.Age >= 24, collectionName);
            Assert.IsTrue(list.Count == 3);
        }

        [TestMethod]
        public void Update()
        {
            BadmatiMongodbHelper.Initialization("mongodb://svcsbx:123qwe@192.168.1.110:14789/Badmati_svc_sbx2", "Badmati_svc_sbx2");

            BadmatiMongodbHelper.UpdateOne<MongoEntity>(a => a.Age == 24, new { Name = "Bob LEe" }, collectionName);
        }
    }

    public class MongoEntity
    { 
        public MongoDB.Bson.ObjectId Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
