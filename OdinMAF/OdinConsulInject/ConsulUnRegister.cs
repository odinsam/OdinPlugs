using System;
using Consul;
using Microsoft.Extensions.Hosting;
using OdinPlugs.OdinInject.Models.ConsulModels;

namespace OdinPlugs.OdinMAF.OdinConsulInject
{
    public class ConsulUnRegister
    {
        public static void OdinConsulUnRegister(IHostApplicationLifetime appLifttime, ConsulModel consulOptions, string consulServiceId)
        {
            appLifttime.ApplicationStopped.Register(() =>
            {
                using (var consulClient = new ConsulClient(c => { c.Address = new Uri($"{consulOptions.Protocol}://{consulOptions.ConsulIpAddress}:{consulOptions.ConsulPort}"); c.Datacenter = consulOptions.DataCenter; }))
                {
#if DEBUG
                    System.Console.WriteLine($"注销服务:{consulServiceId}");
#endif
                    consulClient.Agent.ServiceDeregister(consulServiceId).Wait();
                }
            });
        }
    }
}