using System;
using System.Collections.Generic;
using System.Linq;
using Consul;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OdinPlugs.OdinUtils.Utils.OdinAlgorithm.OdinRandom;

namespace OdinPlugs.OdinMAF.OdinConsulInject.Utils
{
    public class ConsulUtils
    {
        /// <summary>
        /// ~ 自动按服务器配置权重获取需要连接的服务器,客户端负载均衡获取服务的Uri
        /// </summary>
        /// <param name="consulUri">consul的Uri</param>
        /// <param name="serverName">需要获取的服务的名称</param>
        /// <returns></returns>
        public static string ConsulGetServerUri(string consulUri, string serverName)
        {
            using (var consul = new ConsulClient(c => { c.Address = new Uri(consulUri); }))
            {
                var service = consul.Agent.Services().Result.Response;
                var services = service.Values.Where(s => s.Service.Equals(serverName, StringComparison.OrdinalIgnoreCase));
                if (services.Count() > 0)
                {
                    // ~ 创建服务lst集合--------string为服务器的Guid-----int为服务器的权重
                    List<KeyValuePair<AgentService, int>> lstServices = new List<KeyValuePair<AgentService, int>>();
                    foreach (var item in services)
                    {
                        lstServices.Add(new KeyValuePair<AgentService, int>(item, Convert.ToInt32(JsonConvert.DeserializeObject<JObject>(item.Tags[0]).GetValue("Weight"))));
                    }
                    var server = RandomHelper.GetRandomListByWeight<AgentService>(lstServices, 1).First();
                    var s = server.Key;
                    return $"http://{s.Address}:{s.Port}/";
                }
                else
                {
                    throw new Exception("没有找到任何服务!");
                }

            }
        }
    }
}