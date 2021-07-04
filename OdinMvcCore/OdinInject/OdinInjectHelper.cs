using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using OdinPlugs.OdinCore.ConfigModel;
using OdinPlugs.OdinMvcCore.OdinInject.InjectInterface;
using OdinPlugs.OdinMAF.OdinCapService;
using Microsoft.AspNetCore.Http;

namespace OdinPlugs.OdinMvcCore.OdinInject
{
    public static class OdinInjectHelper
    {
        public static IServiceProvider serviceProvider { get; set; }

        public static T GetService<T>() where T : class
        {
            return serviceProvider.GetRequiredService<T>();
        }

        public static T GetService<T>(this IServiceCollection services) where T : class
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<T>();
        }

        public static void SetServiceProvider(this IServiceCollection services)
        {
            serviceProvider = services.BuildServiceProvider();
        }


        /// <summary>
        /// ~ 每次都获取同一个实例
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinSingletonInject(this IServiceCollection services, Assembly ass)
        {
            var registerType = typeof(IAutoInject);
            // ~ 程序中查找 需要注入的切 构造函数没有参数的 所有类型
            var tresult = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && !registerType.IsAssignableFrom(typeof(IAutoInjectWithParamas)));
            foreach (Type t in tresult)
            {
                // ~ 找到所有接口类型
                if (t.IsInterface)
                {
                   
                    // ~ 找到该接口下所有的实现类，即需要注入的类型
                    var types = ass.GetExportedTypes().Where(ts => ts.GetInterfaces().Contains(t)).ToArray();
                    foreach (var tc in types)
                    {
                        // ~ 抽象类不注入
                        if (!tc.Name.StartsWith("Abstract"))
                        {
                            services.AddSingleton(t, tc);
                            System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                        }

                    }
                }
            }
            return services;
        }

        /// <summary>
        /// ~ 每次请求，都获取一个新的实例。即使同一个请求获取多次也会是不同的实例
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinTransientInject(this IServiceCollection services, Assembly ass)
        {
            var registerType = typeof(IAutoInject);
            // ~ 程序中查找 需要注入的切 构造函数没有参数的 所有类型
            var tresult = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && !registerType.IsAssignableFrom(typeof(IAutoInjectWithParamas)));
            foreach (Type t in tresult)
            {
                // ~ 找到所有接口类型
                if (t.IsInterface)
                {
                    // ~ 找到该接口下所有的实现类，即需要注入的类型
                    var types = ass.GetExportedTypes().Where(ts => ts.GetInterfaces().Contains(t)).ToArray();
                    foreach (var tc in types)
                    {
                        // ~ 抽象类不注入
                        if (!tc.Name.StartsWith("Abstract"))
                        {
                            services.AddTransient(t, tc);
                            System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                        }

                    }
                }
            }
            return services;
        }

        /// <summary>
        /// ~ 每次请求，都获取一个新的实例。同一个请求获取多次会得到相同的实例
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinScopedInject(this IServiceCollection services, Assembly ass)
        {
            var registerType = typeof(IAutoInject);
            // ~ 程序中查找 需要注入的切 构造函数没有参数的 所有类型
            var tresult = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && !registerType.IsAssignableFrom(typeof(IAutoInjectWithParamas)));
            foreach (Type t in tresult)
            {
                // ~ 找到所有接口类型
                if (t.IsInterface)
                {
                    // ~ 找到该接口下所有的实现类，即需要注入的类型
                    var types = ass.GetExportedTypes().Where(ts => ts.GetInterfaces().Contains(t)).ToArray();
                    foreach (var tc in types)
                    {
                        // ~ 抽象类不注入
                        if (!tc.Name.StartsWith("Abstract"))
                        {
                            services.AddScoped(t, tc);
                            System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                        }

                    }
                }
            }
            return services;
        }


        /// <summary>
        /// ~ httpClient注入,无证书
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinHttpClient(this IServiceCollection services, string httpClientName)
        {
            var handler = new HttpClientHandler();
            services.AddHttpClient(httpClientName, c =>
            {
            }).ConfigurePrimaryHttpMessageHandler(() => handler);
            System.Console.WriteLine($"注入类型【 httpClient 】无证书注入");
            return services;
        }

        /// <summary>
        /// ~ httpClient注入,有证书
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        /// <param name="ass">需要注册的程序集</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddOdinHttpClientByCer(this IServiceCollection services, Action<List<SslCerOptions>> ActionCers)
        {
            List<SslCerOptions> lstOptions = new List<SslCerOptions>();
            ActionCers(lstOptions);
            for (int i = 0; i < lstOptions.Count; i++)
            {
                if (!string.IsNullOrEmpty(lstOptions[i].CerPath))
                {
                    var handlerWithCer = new HttpClientHandler();
                    var clientCertificate = new X509Certificate2(lstOptions[i].CerPath, lstOptions[i].CerPassword);
                    handlerWithCer.ClientCertificates.Add(clientCertificate);
                    services.AddHttpClient(lstOptions[i].ClientName, c =>
                    {
                    }).ConfigurePrimaryHttpMessageHandler(() => handlerWithCer);
                }
            }
            System.Console.WriteLine($"注入类型【 httpClient 】有证书注入");
            return services;
        }


        public static IServiceCollection AddOdinSingletonInject<T>(this IServiceCollection services, Assembly ass) where T : IAutoInject
        {
            var registerType = typeof(IAutoInject);
            // ~ 程序中查找 需要注入的切 构造函数没有参数的 所有类型
            var tresult = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && !registerType.IsAssignableFrom(typeof(IAutoInjectWithParamas)));
            foreach (Type t in tresult)
            {
                if (t.IsInterface)
                {
                    // t IStudentService
                    var clsResult = ass.GetExportedTypes().Where(c => t.IsAssignableFrom(c) && t != c);
                    foreach (var tc in clsResult)
                    {
                        // ~ 抽象类不注入
                        if (!tc.Name.StartsWith("Abstract"))
                        {
                            // item  StudentService
                            services.AddSingleton(t, provider => Activator.CreateInstance(tc));
                            System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                        }
                    }
                }
            }
            return services;
        }


        public static IServiceCollection AddOdinTransientInject<T>(this IServiceCollection services, Assembly ass) where T : IAutoInject
        {
            var registerType = typeof(IAutoInject);
            // ~ 程序中查找 需要注入的切 构造函数没有参数的 所有类型
            var tresult = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && !registerType.IsAssignableFrom(typeof(IAutoInjectWithParamas)));
            foreach (Type t in tresult)
            {
                if (t.IsInterface)
                {
                    // t IStudentService
                    var clsResult = ass.GetExportedTypes().Where(c => t.IsAssignableFrom(c) && t != c);
                    foreach (var tc in clsResult)
                    {
                        // ~ 抽象类不注入
                        if (!tc.Name.StartsWith("Abstract"))
                        {
                            // item  StudentService
                            services.AddTransient(t, provider => Activator.CreateInstance(tc));
                            System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                        }
                    }
                }
            }
            return services;
        }

        public static IServiceCollection AddOdinScopedInject<T>(this IServiceCollection services, Assembly ass) where T : IAutoInject
        {
            var registerType = typeof(IAutoInject);
            // ~ 程序中查找 需要注入的切 构造函数没有参数的 所有类型
            var tresult = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && !registerType.IsAssignableFrom(typeof(IAutoInjectWithParamas)));
            foreach (Type t in tresult)
            {
                if (t.IsInterface)
                {
                    // t IStudentService
                    var clsResult = ass.GetExportedTypes().Where(c => t.IsAssignableFrom(c) && t != c);
                    foreach (var tc in clsResult)
                    {
                        // ~ 抽象类不注入
                        if (!tc.Name.StartsWith("Abstract"))
                        {
                            // item  StudentService
                            services.AddScoped(t, provider => Activator.CreateInstance(tc));
                            System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                        }
                    }
                }
            }
            return services;
        }

        public static IServiceCollection AddOdinSingletonWithParamasInject<T, TOptions>(this IServiceCollection services, Assembly ass, Action<TOptions> ActionOptions) where T : IAutoInjectWithParamas
        {
            var registerType = typeof(IAutoInjectWithParamas);
            // ~ 程序中查找 需要注入的切 构造函数没有参数的 所有类型
            var t = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && typeof(T) == t).FirstOrDefault();
            if (t != null)
            {
                var tc = ass.GetExportedTypes().Where(c => t.IsAssignableFrom(c) && t != c).FirstOrDefault();
                if (tc != null)
                {
                    var options = Activator.CreateInstance<TOptions>();
                    ActionOptions(options);
                    List<Object> lstObj = new List<object>();
                    foreach (var item in options.GetType().GetProperties())
                    {
                        lstObj.Add(item.GetValue(options));
                    }
                    services.AddSingleton(t, provider => Activator.CreateInstance(tc, lstObj.ToArray()));
                    System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                }
            }
            return services;
        }

        public static IServiceCollection AddOdinSingletonWithParamasInject<T>(this IServiceCollection services, Assembly ass, object[] args) where T : IAutoInjectWithParamas
        {
            var registerType = typeof(IAutoInjectWithParamas);
            // ~ 程序中查找 需要注入的切 构造函数没有参数的 所有类型
            var t = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && typeof(T) == t).FirstOrDefault();
            if (t != null)
            {
                var tc = ass.GetExportedTypes().Where(c => t.IsAssignableFrom(c) && t != c).FirstOrDefault();
                if (tc != null)
                {
                    services.AddSingleton(t, provider => Activator.CreateInstance(tc, args));
                    System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                }
            }
            return services;
        }

        public static IServiceCollection AddOdinTransientWithParamasInject<T, TOptions>(this IServiceCollection services, Assembly ass, Action<TOptions> ActionOptions) where T : IAutoInjectWithParamas
        {
            var registerType = typeof(IAutoInjectWithParamas);
            // ~ 程序中查找 需要注入的切 构造函数没有参数的 所有类型
            var t = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && typeof(T) == t).FirstOrDefault();
            if (t != null)
            {
                var tc = ass.GetExportedTypes().Where(c => t.IsAssignableFrom(c) && t != c).FirstOrDefault();
                if (tc != null)
                {
                    var options = Activator.CreateInstance<TOptions>();

                    ActionOptions(options);
                    List<Object> lstObj = new List<object>();
                    foreach (var item in options.GetType().GetProperties())
                    {
                        lstObj.Add(item.GetValue(options));
                    }
                    services.AddTransient(t, provider => Activator.CreateInstance(tc, lstObj.ToArray()));
                    System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                }
            }
            return services;
        }

        public static IServiceCollection AddOdinTransientWithParamasInject<T>(this IServiceCollection services, Assembly ass, object[] args) where T : IAutoInjectWithParamas
        {
            var registerType = typeof(IAutoInjectWithParamas);
            // ~ 程序中查找 需要注入的切 构造函数没有参数的 所有类型
            var t = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && typeof(T) == t).FirstOrDefault();
            if (t != null)
            {
                var tc = ass.GetExportedTypes().Where(c => t.IsAssignableFrom(c) && t != c).FirstOrDefault();
                if (tc != null)
                {
                    services.AddTransient(t, provider => Activator.CreateInstance(tc, args));
                    System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                }
            }
            return services;
        }


        public static IServiceCollection AddOdinScopedWithParamasInject<T, TOptions>(this IServiceCollection services, Assembly ass, Action<TOptions> ActionOptions) where T : IAutoInjectWithParamas
        {
            var registerType = typeof(IAutoInjectWithParamas);
            var t = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && typeof(T) == t).FirstOrDefault();
            if (t != null)
            {
                var tc = ass.GetExportedTypes().Where(c => t.IsAssignableFrom(c) && t != c).FirstOrDefault();
                if (tc != null)
                {
                    var options = Activator.CreateInstance<TOptions>();
                    ActionOptions(options);
                    List<Object> lstObj = new List<object>();
                    foreach (var item in options.GetType().GetProperties())
                    {
                        lstObj.Add(item.GetValue(options));
                    }
                    services.AddScoped(t, provider => Activator.CreateInstance(tc, lstObj.ToArray()));
                    System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                }
            }
            return services;
        }


        public static IServiceCollection AddOdinScopedWithParamasInject<T, TOptions>(this IServiceCollection services, Assembly ass, object[] args) where T : IAutoInjectWithParamas
        {
            var registerType = typeof(IAutoInjectWithParamas);
            var t = ass.GetExportedTypes().Where(t => registerType.IsAssignableFrom(t) && t != registerType && t != registerType && typeof(T) == t).FirstOrDefault();
            if (t != null)
            {
                var tc = ass.GetExportedTypes().Where(c => t.IsAssignableFrom(c) && t != c).FirstOrDefault();
                if (tc != null)
                {
                    services.AddScoped(t, provider => Activator.CreateInstance(tc, args));
                    System.Console.WriteLine($"注入类型【 {tc} 】,接口:【 {t} 】,名称:【 {tc.Name} 】");
                }
            }
            return services;
        }

        /// <summary>
        /// cap注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="mysqlConnectionString">mysqlConnectionString</param>
        /// <param name="rabbitMQOptions">RabbitMQOptions</param>
        /// <param name="mongoConnectionString">注意，仅支持MongoDB 4.0+集群</param>
        /// <returns></returns>
        public static IServiceCollection AddOdinCapInject(this IServiceCollection services, OdinCapEventBusOptions odinCapEventBusOptions)
        {
            services.AddCap(x =>
            {
                //如果你使用的ADO.NET，根据数据库选择进行配置：
                // x.UseSqlServer("数据库连接字符串");
                x.UseMySql(odinCapEventBusOptions.MysqlConnectionString);
                // x.UsePostgreSql("数据库连接字符串");
                //如果你使用的 MongoDB，你可以添加如下配置：
                if (!string.IsNullOrEmpty(odinCapEventBusOptions.MongoConnectionString))
                    x.UseMongoDB(odinCapEventBusOptions.MongoConnectionString);  //注意，仅支持MongoDB 4.0+集群

                //CAP支持 RabbitMQ、Kafka、AzureServiceBus 等作为MQ，根据使用选择配置：
                if (odinCapEventBusOptions.RabbitmqOptions != null)
                {
                    x.UseRabbitMQ(rb =>
                        {
                            rb.HostName = string.Join(",", odinCapEventBusOptions.RabbitmqOptions.HostNames);
                            rb.UserName = odinCapEventBusOptions.RabbitmqOptions.Account.UserName;
                            rb.Password = odinCapEventBusOptions.RabbitmqOptions.Account.Password;
                            rb.VirtualHost = odinCapEventBusOptions.RabbitmqOptions.VirtualHost;
                            rb.Port = odinCapEventBusOptions.RabbitmqOptions.Port;
                        });
                }
                // x.UseKafka("ConnectionStrings");
                // x.UseAzureServiceBus("ConnectionStrings");
                x.UseDashboard();
            });
            System.Console.WriteLine($"注入类型【 cap 】注入");
            return services;
        }
    }
}