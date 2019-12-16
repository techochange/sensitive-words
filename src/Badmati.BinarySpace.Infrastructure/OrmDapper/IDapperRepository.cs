using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Badmati.BinarySpace.Infrastructure.OrmDapper
{
    public interface IDapperRepository
    {

    }

    public interface IDapperRepository<T> : IDapperRepository where T : class, new()
    {
        /// <summary>
        /// 得到单个实体，根据主键查询
        /// 默认id字段如果是其他类型的字段需要使用[Key]标注
        /// </summary>
        /// <param name="key">根据主键查询</param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        /// <remarks>
        /// var model=GetEntity(32)
        /// sql=>select * from table where [id]=32
        /// </remarks>
        Task<T> GetByKeyAsync(object key, IDbTransaction transaction = null, int? commandTimeout = null);

        /// <summary>
        /// 得到单个实体，根据主键查询
        /// 默认id字段如果是其他类型的字段需要使用[Key]标注
        /// </summary>
        /// <param name="param">根据条件查询</param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        /// <remarks>
        /// var model=GetEntity(32)
        /// sql=>select * from table where [id]=32
        /// </remarks>
        Task<T> GetFirstAsync(object param, IDbTransaction transaction = null, int? commandTimeout = null);

        /// <summary>
        /// 根据查询条件得到实体集合
        /// </summary>
        /// <param name="whereConditions">动态对象new{}指定查询条件</param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <remarks>
        /// var models=GetListAsync(new { Age=10 });
        /// => select * from table where Age=10
        /// </remarks>
        /// <returns></returns>
        Task<IEnumerable<T>> GetListAsync(object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null);

        /// <summary>
        /// 根据查询条件得到实体集合
        /// </summary>
        /// <param name="whereConditions">指定查询条件字符串</param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="parameters"></param> 
        /// <returns></returns>
        /// <remarks>
        /// var models=await GetListAsync("where age > @Age",new { Age=10 });
        /// </remarks>
        Task<IEnumerable<T>> GetListAsync(string whereConditions="", object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null);

        /// <summary>
        /// 得到分页实体集合
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="conditions"></param>
        /// <param name="orderby"></param>
        /// <param name="parameters"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="transaction"></param>
        /// <remarks>
        /// var models=await GetPageListAsync(1,10,"where age=10 and name like '%a%' ","name desc");
        /// </remarks>
        /// <returns></returns>
        Task<IEnumerable<T>> GetPageListAsync(int page, int size, string whereConditions="", string orderby="", object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null);

        /// <summary>
        /// 插入数据 返回成功与否
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        Task<bool> InsertAsync(T entity, IDbTransaction transaction = null, int? commandTimeout = null);

        /// <summary>
        /// 插入数据 返回成功与否
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        Task<bool> SaveAsync(T entity, IDbTransaction transaction = null, int? commandTimeout = null);

        /// <summary>
        /// 插入数据 返回插入数据的Key
        /// </summary>
        /// <typeparam name="Tkey"></typeparam>
        /// <param name="entity"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        Task<Tkey> InsertAsync<Tkey>(T entity, IDbTransaction transaction = null, int? commandTimeout = null);

        /// <summary>
        /// 更新数据,匹配Key,修改属性
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(T entity, IDbTransaction transaction = null, int? commandTimeout = null);

        /// <summary>
        /// 获取某个条件的总记录数
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <remarks>
        /// var models=await GetCountAsync("where age=@age",new {age=20});
        /// </remarks>
        /// <returns></returns>
        Task<int> GetCountAsync(string conditions="", object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null);

        /// <summary>
        /// 获取某个条件的总记录数
        /// </summary>
        /// <param name="conditions">new { Name="lee" }</param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <remarks>
        /// var models=await GetCountAsync(new {age=20});
        /// </remarks>
        /// <returns></returns>
        Task<int> GetCountAsync(object conditions, IDbTransaction transaction = null, int? commandTimeout = null);

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        Task<T1> ExecuteTransaction<T1>(Func<IDbTransaction, Task<T1>> func);

        Task<int> DeleteAsync(T entity, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<int> DeleteAsync(object conditon, IDbTransaction transaction = null, int? commandTimeout = null);
        /// <summary>
        /// 数据更新
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <param name="commandTimeOut"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(string sqlString, object param, CommandType commandType = CommandType.Text, int? commandTimeOut = 5);
    }
}
