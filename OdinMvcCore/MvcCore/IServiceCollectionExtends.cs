using Microsoft.Extensions.DependencyInjection;

namespace OdinPlugs.OdinMvcCore.MvcCore
{
    public static class IServiceCollectionExtends
    {
        /// <summary>
        /// 从以注册的服务中找到对应的服务驱动
        /// </summary>
        /// <param name="services"></param>
        /// <typeparam name="T">需要获取的服务类型</typeparam>
        /// <returns>服务驱动</returns>
        public static T GetRegisteredRequiredService<T>(this IServiceCollection services) where T : notnull
        {
            var provider = services.BuildServiceProvider();
            return provider.GetRequiredService<T>();
        }
    }
}