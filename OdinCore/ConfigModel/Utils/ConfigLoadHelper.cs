using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using OdinPlugs.OdinUtils.OdinFiles;

namespace OdinPlugs.OdinCore.ConfigModel.Utils
{
    public class ConfigLoadHelper
    {


        public static void LoadConfigs(string env, string currentPath, IConfigurationBuilder config, string rootPath)
        {
            // ~ 加载 *.json配置文件
            LoadConfigFiles(currentPath, config, rootPath);
            LoadConfigFilesByEnv(Path.Combine(currentPath, env), config, rootPath);

        }
        public static void LoadConfigFiles(string currentPath, IConfigurationBuilder config, string rootPath)
        {
            foreach (var item in Directory.GetFiles(currentPath))
            {
                var fileName = Path.GetFileName(item);
                if (fileName != "cnf.json" && Path.GetExtension(item).EndsWith(".json"))
                {
                    var configPath = item.Replace(rootPath, "");
                    // 判断 是否是 windows 平台
                    // var jsonConfigPath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? configPath : item;
                    if (File.Exists(item))
                    {
                        System.Console.WriteLine(item);
                        //config.add 加载的文件 路径需要是相对路径 而不能是绝对路径
                        config.Add(new JsonConfigurationSource { Path = configPath, Optional = false, ReloadOnChange = true });
                    }
                }
            }
        }
        public static void LoadConfigFilesByEnv(string currentPath, IConfigurationBuilder config, string rootPath)
        {
            LoadConfigFiles(currentPath, config, rootPath);
            var dir = Directory.GetDirectories(currentPath);
            if (dir != null && dir.Length > 0)
            {
                foreach (var dircItem in dir)
                {
                    if (!Path.GetDirectoryName(dircItem).EndsWith(Path.Combine(FileHelper.DirectorySeparatorChar, "envConfig")))
                        LoadConfigFilesByEnv(dircItem, config, rootPath);
                }
            }
        }
    }
}