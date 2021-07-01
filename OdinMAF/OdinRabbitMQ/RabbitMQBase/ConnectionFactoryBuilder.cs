using RabbitMQ.Client;

namespace OdinPlugs.OdinMAF.OdinRabbitMQ.RabbitMQBase
{
    public class ConnectionFactoryBuilder
    {
        private static ConnectionFactory connectionFactory = null;
        public static ConnectionFactory GetOrCreateConnectionFactory(string userName, string password)
        {
            if (connectionFactory == null)
            {
                connectionFactory = new ConnectionFactory();
                connectionFactory.UserName = userName;
                connectionFactory.Password = password;
                connectionFactory.AutomaticRecoveryEnabled = true;
                connectionFactory.TopologyRecoveryEnabled = true;
            }
            return connectionFactory;
        }
    }
}