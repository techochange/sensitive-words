using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BMT.Server.Infrastructure
{
    /// <summary>
    /// mongodb 操作类 基于官方driver
    /// https://mongodb.github.io/mongo-csharp-driver/2.7/getting_started/quick_tour/
    /// 
    /// </summary>
    public static class BmdMongodbHelper
    {
        private static IMongoClient _mongoClient;

        private static string _defaultDbName;

        /// <summary>
        /// 初始化mnogodb连接字符串
        /// </summary>
        /// <param name="mongodbConnection"></param>
        /// <param name="dbname"></param>
        public static void Initialization(string mongodbConnection,string dbname)
        {
            _mongoClient = new MongoClient(mongodbConnection);
            _defaultDbName = dbname;
        }

        /// <summary>
        /// 插入数据到mongodb
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document"></param>
        /// <param name="colname"></param>
        /// <param name="dbname"></param>
        public static void Insert<T>(T document, string colname, string dbname = "")
        {
            var collections = GetMongoCollection<T>(colname, dbname);
            collections.InsertOne(document);
        }

        /// <summary>
        /// 异步插入数据到mongodb
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document"></param>
        /// <param name="colname"></param>
        /// <param name="dbname"></param>
        /// <returns></returns>
        public async static Task InsertAsync<T>(T document, string colname, string dbname = "")
        {
            var collections = GetMongoCollection<T>(colname, dbname);
            await collections.InsertOneAsync(document);
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document"></param>
        /// <param name="colname"></param>
        /// <param name="dbname"></param>
        public static void InsertMany<T>(List<T> document, string colname, string dbname = "")
        {
            var collections = GetMongoCollection<T>(colname, dbname);
            collections.InsertMany(document);
        }

        /// <summary>
        /// 异步批量插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document"></param>
        /// <param name="colname"></param>
        /// <param name="dbname"></param>
        /// <returns></returns>
        public async static Task InsertManyAsync<T>(List<T> document, string colname, string dbname = "")
        {
            var collections = GetMongoCollection<T>(colname, dbname);
            await collections.InsertManyAsync(document);
        }

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="colname"></param>
        /// <param name="dbname"></param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string colname,string dbname = "")
        {
            var collections = GetMongoCollection<T>(colname, dbname);
            return collections.Find(new BsonDocument()).ToList();
        }

        /// <summary>
        /// 异步查询全部
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="colname"></param>
        /// <param name="dbname"></param>
        /// <returns></returns>
        public async static Task<List<T>> GetAllAsync<T>(string colname, string dbname = "")
        {
            var collections = GetMongoCollection<T>(colname, dbname);
            return await collections.Find(new BsonDocument()).ToListAsync();
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">查询表达式</param>
        /// <param name="colname">集合名</param>
        /// <param name="dbname">数据库名</param>
        /// <param name="sort">排序字段</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        public static List<T> Query<T>(Expression<Func<T, bool>> expression, string colname, string dbname = "", string sort = "_id", bool isAsc = true)
        {
            var collections = GetMongoCollection<T>(colname, dbname);
            SortDefinition<T> tempSort = null;
            if (isAsc)
            {
                tempSort = Builders<T>.Sort.Ascending(sort);
            }
            else
            {
                tempSort = Builders<T>.Sort.Descending(sort);
            }
            return collections.Find(expression).Sort(tempSort).ToList();
        }

        /// <summary>
        /// 异步根据条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="colname"></param>
        /// <param name="dbname"></param>
        /// <param name="sort"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public async static Task<List<T>> QueryAsync<T>(Expression<Func<T, bool>> expression, string colname, string dbname = "", string sort = "_id", bool isAsc = true)
        {
            SortDefinition<T> tempSort = null;
            if (isAsc)
            {
                tempSort = Builders<T>.Sort.Ascending(sort);
            }
            else
            {
                tempSort = Builders<T>.Sort.Descending(sort);
            }
            var collections = GetMongoCollection<T>(colname, dbname);
            return await collections.Find(expression).Sort(tempSort).ToListAsync();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="updateObj">new { Age=20,Name="adison" }</param>
        /// <param name="colname"></param>
        /// <param name="isMutil">是否批量更新</param>
        /// <param name="dbname"></param>
        /// <remarks>
        /// updateObj 需要更新的动态对象
        /// new { Age=20,Name="adison" }
        /// </remarks>
        public static void Update<T>(Expression<Func<T, bool>> expression,object updateObj, string colname,bool isMutil=false, string dbname = "")
        {
            var jsonStr = updateObj.Json2Str(false);
            var dic = jsonStr.Str2Json<Dictionary<string, object>>();
            var collections = GetMongoCollection<T>(colname, dbname);
            var update = Builders<T>.Update;
            UpdateDefinition<T> updateDef = null;
            foreach (var item in dic)
            {
                if (updateDef == null)
                {
                    updateDef = update.Set(item.Key, item.Value);
                }
                else
                {
                    updateDef.Set(item.Key, item.Value);
                }
            }
            if (isMutil)
            {
                collections.UpdateMany(expression, updateDef);
            }
            else
            {
                collections.UpdateOne(expression, updateDef);
            } 
        }

        /// <summary>
        /// 异步更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="updateObj">new { Age=20,Name="adison" }</param>
        /// <param name="colname"></param>
        /// <param name="isMutil">是否批量更新</param>
        /// <param name="dbname"></param>
        /// <returns></returns>
        public async static Task UpdateAsync<T>(Expression<Func<T, bool>> expression, object updateObj, string colname, bool isMutil = false, string dbname = "")
        {
            var jsonStr = updateObj.Json2Str(false);
            var dic = jsonStr.Str2Json<Dictionary<string, object>>();
            var collections = GetMongoCollection<T>(colname, dbname);
            var update = Builders<T>.Update;
            UpdateDefinition<T> updateDef = null;
            foreach (var item in dic)
            {
                if (updateDef == null)
                {
                    updateDef = update.Set(item.Key, item.Value);
                }
                else
                {
                    updateDef.Set(item.Key, item.Value);
                }
            }
            if (isMutil)
            {
                await collections.UpdateManyAsync(expression, updateDef);
            }
            else
            {
                await collections.UpdateOneAsync(expression, updateDef);
            }
        }

        private static IMongoCollection<T> GetMongoCollection<T>(string colname,string dbname = "")
        {
            if(string.IsNullOrWhiteSpace(_defaultDbName))
            {
                throw new ArgumentNullException("_defaultDbName", "Mongodb 链接字符串必须先初始化");
            }

            var tempdb = string.IsNullOrWhiteSpace(dbname) ? _defaultDbName : dbname;
            var db = _mongoClient.GetDatabase(tempdb);
            return db.GetCollection<T>(colname);
        }
    }
}
