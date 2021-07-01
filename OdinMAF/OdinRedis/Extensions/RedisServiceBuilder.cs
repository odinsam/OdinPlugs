using Microsoft.Extensions.DependencyInjection;

namespace OdinPlugs.OdinMAF.OdinRedis.Extensions
{
    public class RedisServiceBuilder
    {
        public string redisConnectionString { get; set; }
        public string redisInstanceName { get; set; }
        public IServiceCollection ServiceCollection { get; set; }

        public RedisServiceBuilder(IServiceCollection services)
        {
            ServiceCollection = services;
        }

        public void UseRedisCache(string redisConnectionString, string redisInstanceName)
        {
            ServiceCollection.AddSingleton(new OdinRedisCache(redisConnectionString, redisInstanceName));
        }
    }
}