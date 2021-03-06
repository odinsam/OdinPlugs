using Mapster;
using Microsoft.Extensions.DependencyInjection;
using OdinPlugs.OdinCore.ConfigModel;
using OdinPlugs.OdinInject.InjectPlugs;
using OdinPlugs.OdinInject.Models.RabbitmqModels;
using OdinPlugs.OdinInject.Models.RedisModels;
using OdinPlugs.OdinUtils.OdinExtensions.BasicExtensions.OdinObject;
using OdinPlugs.SnowFlake.Inject;

namespace OdinPlugs.OdinMAF.OdinInject
{
    public static class OdinInjectExtensions
    {
        /// <summary>
        /// 注入常用的接口 - SnowFlake、MongoDb、Redis、CacheManager、CapEventBus、Canal、CapInject
        /// </summary>
        /// <param name="services"></param>
        /// <param name="_Options"></param>
        /// <returns></returns>
        public static IServiceCollection AddOdinInject(this IServiceCollection services, ConfigOptions _Options)
        {
            services
                .AddSingletonSnowFlake(_Options.FrameworkConfig.SnowFlake.DataCenterId, _Options.FrameworkConfig.SnowFlake.WorkerId)
                .AddOdinTransientMongoDb(
                    opt => { opt.ConnectionString = _Options.MongoDb.MongoConnection; opt.DbName = _Options.MongoDb.Database; })
                .AddOdinTransientRedis(
                    opt => { opt.ConnectionString = _Options.Redis.Connection; opt.InstanceName = _Options.Redis.InstanceName; })
                .AddOdinTransientCacheManager(
                    opt =>
                    {
                        opt.OptCm = _Options.CacheManager.Adapt<OdinPlugs.OdinInject.Models.CacheManagerModels.CacheManagerModel>();
                        opt.OptRbmq = _Options.Redis.Adapt<RedisModel>();
                    })
                .AddOdinSingletonCapEventBus()
                .AddOdinSingletonCanal()
                .AddOdinCapInject(opt =>
                {
                    opt.MysqlConnectionString = _Options.DbEntity.ConnectionString;
                    opt.RabbitmqOptions = _Options.RabbitMQ.Adapt<RabbitMQOptions>();
                });
            return services;
        }
    }
}