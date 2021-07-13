using System.Collections.Generic;
using Newtonsoft.Json;
using OdinPlugs.OdinUtils.OdinJson.ContractResolver;

namespace OdinPlugs.OdinCore.Models.Aop
{
    public class Aop_Invoker_Model
    {
        /// <summary>
        /// 自动编号,与链路编号一致
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(JsonConverterLong))]
        public long? Id { get; set; }

        public string GUID { get; set; } = "";

        /// <summary>
        /// ControllerName
        /// </summary>
        /// <returns></returns>
        public string ControllerName { get; set; }

        /// <summary>
        /// ActionName
        /// </summary>
        /// <returns></returns>
        public string ActionName { get; set; }

        public string RequestUrl { get; set; }

        public Dictionary<string, string> RequestHeader { get; set; }

        /// <summary>
        /// 入参
        /// </summary>
        /// <returns></returns>
        public string InputParams { get; set; }

        /// <summary>
        /// 调用方式
        /// </summary>
        /// <returns></returns>
        public string ApiMethod { get; set; }

        public string ApiBeginTime { get; set; }
        public long? BeginTime { get; set; }

        public long? EndTime { get; set; }
        public string ApiEndTime { get; set; }
        public long? ElaspedTime { get; set; }
        public string Author { get; set; }
    }
}