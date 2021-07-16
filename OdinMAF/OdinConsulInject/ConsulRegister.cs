using System;
using Consul;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OdinPlugs.OdinInject.Models.ConsulModels;

namespace OdinPlugs.OdinMAF.OdinConsulInject
{
    public class ConsulRegister
    {
        private static string consulServiceId;
        public static string OdinConsulRegister(ConsulModel consulOptions, DoMainModel doMainOptions)
        {
            consulServiceId = consulOptions.ConsulName + "-" + Guid.NewGuid().ToString("N");
            using (var consulClient = new ConsulClient(c => { c.Address = new Uri($"{consulOptions.Protocol}://{consulOptions.ConsulIpAddress}:{consulOptions.ConsulPort}"); c.Datacenter = consulOptions.DataCenter; }))
            {
                AgentServiceRegistration asr = new AgentServiceRegistration();
                JObject jobj = new JObject();
                jobj.Add("Id", consulServiceId);
                jobj.Add("Weight", consulOptions.Weight);
                consulClient.Agent.ServiceRegister(new AgentServiceRegistration
                {
                    Name = consulOptions.ConsulName,
                    Address = doMainOptions.IpAddress,
                    Port = doMainOptions.Port,
                    ID = consulServiceId,
                    Tags = new string[] { JsonConvert.SerializeObject(jobj) },
                    Check = new AgentServiceCheck
                    {
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(consulOptions.ConsulCheck.DeregisterCriticalServiceAfter),
                        Interval = TimeSpan.FromSeconds(consulOptions.ConsulCheck.Interval),
                        HTTP = consulOptions.ConsulCheck.HealthApi,
                        Timeout = TimeSpan.FromSeconds(consulOptions.ConsulCheck.Timeout)
                    }
                }).Wait();
                return consulServiceId;
            }
        }

    }
}