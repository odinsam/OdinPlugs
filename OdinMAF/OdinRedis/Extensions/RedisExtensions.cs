using System;
using Microsoft.Extensions.DependencyInjection;

namespace OdinPlugs.OdinMAF.OdinRedis.Extensions
{
    public static class RedisExtensions
    {
        public static void AddRedisCache(this IServiceCollection services, Action<RedisServiceBuilder> options)
        {
            var builder = new RedisServiceBuilder(services);
            options(builder);
        }
    }
}