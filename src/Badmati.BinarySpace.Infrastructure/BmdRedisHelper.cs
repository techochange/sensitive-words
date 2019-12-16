using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badmati.BinarySpace.Infrastructure
{
    /// <summary>
    /// redis 缓存 静态类 基于csredis https://github.com/2881099/csredis
    /// 使用之前必须先初始化 Initialization
    /// </summary>
    public static class BmdRedisHelper
    { 
        public static string KeyPrefix { get; set; }
        public static void Initialization(string redisConnection,string keyPrefix)
        {
            KeyPrefix = keyPrefix;
            RedisHelper.Initialization(new CSRedis.CSRedisClient(redisConnection));
        } 

        /// <summary>
        /// 直接获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            key = KeyPrefix + key;
            return RedisHelper.Get<T>(key);
        }
         
        /// <summary>
        /// 缓存壳,如果没有就设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="timeoutSecond"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T CacheShell<T>(string key,int timeoutSecond,Func<T> func)
        {
            key = KeyPrefix + key;
            return RedisHelper.CacheShell(key, timeoutSecond, func);
        }
		/// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static long DelCache(string[] keys)
        {
            return RedisHelper.Del(keys.Select(s=> KeyPrefix + s).ToArray());
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long DelCacheByKey(string key)
        {
            key = KeyPrefix + key;
            return RedisHelper.Del(new string[] { key });
        }

        /// <summary>
        /// 将一个或多个值插入到列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public static long RPush<T>(string key, T member)
        {
            key = KeyPrefix + key;
            return RedisHelper.RPush(key, member);
        }

        /// <summary>
        /// 将一个或多个值插入到列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public static async Task<long> RPushAsync<T>(string key, T member)
        {
            key = KeyPrefix + key;
            return await RedisHelper.RPushAsync(key, member);
        }

        /// <summary>
        /// 对一个列表进行修剪，只保留指定区间内的数据
        /// 0表示开始位置 -1表示结束位置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static async Task<bool> LTrimAsync(string key, long start, long stop)
        {
            key = KeyPrefix + key;
            return await RedisHelper.LTrimAsync(key, start, stop);
        }


        /// <summary>
        /// 通过索引设置列表元素的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task<bool> LSetAsync(string key, long index, object value)
        {
            key = KeyPrefix + key;
            return await RedisHelper.LSetAsync(key, index, value);
        }
        /// <summary>
        /// 获取一个列表指定区间内的数据
        /// 0表示开始位置 -1表示结束位置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static async Task<T[]> GetRange<T>(string key, long start, long stop)
        {
            key = KeyPrefix + key;
            return await RedisHelper.LRangeAsync<T>(key, start, stop);
        }
    }
}
