using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Badmati.BinarySpace.Infrastructure
{
    /// <summary>
    /// json 序列化扩展
    /// </summary>
    public static class JsonExtensionHelper
    {
        /// <summary>
        /// 对象转换为字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Json2Str(this object obj, bool isCame = true)
        {
            var setting = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            if (isCame)
            {
                return JsonConvert.SerializeObject(obj, setting);
            }
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 字符串转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T Str2Json<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
