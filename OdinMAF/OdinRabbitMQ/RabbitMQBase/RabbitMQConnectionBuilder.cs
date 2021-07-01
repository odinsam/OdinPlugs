using RabbitMQ.Client;

namespace OdinPlugs.OdinMAF.OdinRabbitMQ.RabbitMQBase
{
    public class RabbitMQConnectionBuilder
    {
        public static IConnection CreateRabbitMQConnection(string userName, string password, string[] topoIps = null)
        {
            ConnectionFactory connectionFactory = ConnectionFactoryBuilder.GetOrCreateConnectionFactory(userName, password);
            var connection = connectionFactory.CreateConnection(topoIps);
            return connection;
        }
    }
}