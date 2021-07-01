using System;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Odin.Plugs.OdinCore.Configs
{
    public static class ConfigHelper
    {
        private static readonly IConfiguration _configuration;

        static ConfigHelper()
        {
            //在当前目录或者根目录中寻找appsettings.json文件
            var fileName = "ServiceConfig/cnf.json";

            var directory = AppContext.BaseDirectory;
            directory = directory.Replace("\\", "/");

            var filePath = $"{directory}/{fileName}";
            if (!File.Exists(filePath))
            {
                var length = directory.IndexOf("/bin", StringComparison.Ordinal);
                filePath = $"{directory.Substring(0, length)}/{fileName}";
            }

            var builder = new ConfigurationBuilder()
                .AddJsonFile(filePath, false, true);

            _configuration = builder.Build();
        }

        public static IConfigurationSection GetSection(string key)
        {
            return _configuration.GetSection(key);
        }

        public static T LoadConfig<T>(string configFullPath)
        {
            var builder = new ConfigurationBuilder().AddJsonFile(configFullPath).Build();
            T config = builder.Get<T>();
            return config;
        }

        public static T GetConfig<T>(string configPath, string fileName)
        {
            foreach (var item in Directory.GetFiles(configPath))
            {
                if (Path.GetFileName(item.ToLower()) == fileName.ToLower())
                {
                    return ConfigHelper.LoadConfig<T>(item);
                }
            }
            return default(T);
        }
    }
}