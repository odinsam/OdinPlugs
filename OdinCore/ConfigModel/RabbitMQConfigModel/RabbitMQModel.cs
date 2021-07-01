using System;

namespace Odin.Plugs.OdinCore.ConfigModel.RabbitMQConfigModel
{
    public class ArgumentsModel
    {
        public string KeyName { get; set; }
        public string Value { get; set; }
    }
    public class QueuesModel
    {
        public string QueuesName { get; set; }
        public bool Durable { get; set; } = true;
        public string RoutingKey { get; set; }
        public bool AutoDelete { get; set; } = false;
        public bool Exclusive { get; set; } = false;
        public ArgumentsModel[] Arguments { get; set; }
    }
    public class ExchangeModel
    {
        public string ExchangeName { get; set; }
        public string ExchangeType { get; set; }
        public bool Durability { get; set; } = true;
        public bool AutoDelete { get; set; } = false;
        public ArgumentsModel[] Arguments { get; set; }
        public QueuesModel[] Queues { get; set; }
    }
    public class RabbitMQAccount
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class RabbitMQOptions
    {
        public RabbitMQAccount Account { get; set; }
        public string[] HostNames { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public ExchangeModel[] Exchanges { get; set; }
    }
}