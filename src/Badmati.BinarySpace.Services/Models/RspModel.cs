using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Badmati.BinarySpace.Services.Models
{
    public class RpsModel<T> : RpsBaseModel
    {
        public T Data { get; set; }
    }

    public class RpsModelTermOrFestivalModel<T> : RpsModel<T>
    {
        public string Sign { get; set; }
    }

    public class RpsListModel<T> : RpsBaseModel
    {
        public int Total { get; set; }
        public List<T> List { get; set; }
    }

    public class RpsBaseModel
    {
        [JsonProperty("resultCode")]
        public int Status { get; set; }
        public string Msg { get; set; }
        /// <summary>
        /// 追踪id（如果出错的话）
        /// </summary>
        public string TraceId { get; set; }

        public bool IsSuccess { get; set; } = true;
    }

    /// <summary>
    /// 含总数的列表数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListData<T>
    {
        public int Total { get; set; }

        public List<T> List { get; set; }
    }
}
