using OdinPlugs.OdinCore.ConfigModel.RabbitMQConfigModel;

namespace OdinPlugs.OdinMAF.OdinCapService
{
    public class OdinCapEventBusOptions
    {
        public string MysqlConnectionString { get; set; }
        public RabbitMQOptions RabbitmqOptions { get; set; }
        public string MongoConnectionString { get; set; }
    }
}