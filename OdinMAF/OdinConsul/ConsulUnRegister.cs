using System;
using Consul;
using Microsoft.Extensions.Hosting;
using OdinPlugs.OdinCore.ConfigModel.ConsulModel;

namespace OdinPlugs.OdinMAF.OdinConsul
{
    public class ConsulUnRegister
    {
        public static void OdinConsulUnRegister(IHostApplicationLifetime appLifttime, ConsulOptions consulOptions, string consulServiceId)
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