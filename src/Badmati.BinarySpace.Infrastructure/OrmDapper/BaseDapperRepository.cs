using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
//using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Badmati.BinarySpace.Infrastructure.OrmDapper
{
    public class BaseDapperRepository<T> : IDapperRepository<T> where T : class, new()
    {

        //private readonly IConfiguration _Configuration;
        private readonly string _ConnectionStr;

        public BaseDapperRepository(IConfiguration configuration)
        {
            //this._Configuration = configuration;
            this._ConnectionStr = SystemConfig.DB_CONNECTION_STRTING;//this._Configuration["ConnectionStr"];
        }

        public async Task<T1> ExecuteTransaction<T1>(Func<IDbTransaction, Task<T1>> func)
        {
            using (IDbTransaction transaction = GetCon().BeginTransaction())
            {
                return await func(transaction);
            }
        }

        public async Task<T> GetByKeyAsync(object key, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using (var con = GetCon())
            {
                return await con.GetAsync<T>(key, transaction, commandTimeout);
            }
        }

        public async Task<T> GetFirstAsync(object param, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using (var con = GetCon())
            {
                return (await con.GetListAsync<T>(param, transaction, commandTimeout)).FirstOrDefault();
            }
        }

        public async Task<int> GetCountAsync(string conditions="", object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using (var con = GetCon())
            {
                return await con.RecordCountAsync<T>(conditions, parameters, transaction, commandTimeout);
            }
        }

        public async Task<int> GetCountAsync(object conditions, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using (var con = GetCon())
            {
                return await con.RecordCountAsync<T>(conditions, transaction, commandTimeout);
            }
        }

        public async Task<IEnumerable<T>> GetListAsync(object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using (var con = GetCon())
            {
                return await con.GetListAsync<T>(whereConditions, transaction, commandTimeout);
            }
        }

        public async Task<IEnumerable<T>> GetListAsync(string whereConditions="", object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using (var con = GetCon())
            {
                return await con.GetListAsync<T>(whereConditions, parameters, transaction, commandTimeout);
            }
        }

        public async Task<IEnumerable<T>> GetPageListAsync(int page, int size, string conditions="", string orderby="", object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using (var con = GetCon())
            {
                return await con.GetListPagedAsync<T>(page, size, conditions, orderby, parameters, transaction, commandTimeout);
            }
        }
        public async Task<bool> SaveAsync(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using (var con = GetCon())
            {
                var rows = await con.UpdateAsync<T>(entity, transaction, commandTimeout);
                if (rows < 1)
                {
                    var affect = await con.InsertAsync<T>(entity, transaction, commandTimeout);
                    return affect.HasValue && affect.Value > 0;
                }
                else
                {
                    return true;
                }
            }
        }
        public async Task<bool> InsertAsync(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using (var con = GetCon())
            {
                var affect = await con.InsertAsync<T>(entity, transaction, commandTimeout);
                return affect.HasValue && affect.Value > 0;
            }
        }

        public async Task<TKey> InsertAsync<TKey>(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using (var con = GetCon())
            {
                return await con.InsertAsync<TKey, T>(entity, transaction, commandTimeout);
            }
        }

        public async Task<int> UpdateAsync(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using (var con = GetCon())
            {
                return await con.UpdateAsync<T>(entity, transaction, commandTimeout);
            }
        }
        public async Task<bool> UpdateAsync(string sqlString, object param, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {
            return await ExecuteNonQueryAsync(sqlString, param, commandType, commandTimeOut);
        }


        private async Task<bool> ExecuteNonQueryAsync(string sqlString, object param, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {
            var intResult = 0;
            using (var db = GetCon() )
            {
                if (null == param)
                {
                    intResult = await db.ExecuteAsync(sqlString, null, null, commandTimeOut, commandType);

                }
                else
                {
                    intResult = await db.ExecuteAsync(sqlString, param, null, commandTimeOut, commandType);
                }
            }

            return intResult > 0;
        }

        /// <summary>
        /// 获取数据库链接,mysql
        /// </summary>
        /// <returns></returns>
        protected virtual IDbConnection GetCon()
        {
            return new MySqlConnection(this._ConnectionStr);
        }

        public async Task<int> DeleteAsync(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using (var con = GetCon())
            {
                return await con.DeleteAsync<T>(entity, transaction, commandTimeout);
            }
        }


        public async Task<int> DeleteAsync(object conditon, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using (var con = GetCon())
            {
                return await con.DeleteListAsync<T>(conditon, transaction, commandTimeout);
            }
        }
    }
}
