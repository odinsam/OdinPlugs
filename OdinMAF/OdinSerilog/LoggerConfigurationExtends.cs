using System.Linq;
using OdinPlugs.OdinMAF.OdinSerilog.Models;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace OdinPlugs.OdinMAF.OdinSerilog
{
    public static class LoggerConfigurationExtends
    {
        public static LoggerConfiguration OdinWriteLog(this LoggerConfiguration loggerConfiguration, LogWriteFileModel logWriteFileModel, LogWriteToConsoleModel logWriteToConsole, LogWriteMySqlModel logWriteMySqlModel = null)
        {
            return loggerConfiguration
                .WriteTo.OdinWrite(
                    LogEventLevel.Debug, logWriteFileModel, logWriteToConsole, logWriteMySqlModel
                )
                .WriteTo.OdinWrite(
                    LogEventLevel.Error, logWriteFileModel, logWriteToConsole, logWriteMySqlModel
                )
                .WriteTo.OdinWrite(
                    LogEventLevel.Fatal, logWriteFileModel, logWriteToConsole, logWriteMySqlModel
                )
                .WriteTo.OdinWrite(
                    LogEventLevel.Information, logWriteFileModel, logWriteToConsole, logWriteMySqlModel
                )
                .WriteTo.OdinWrite(
                    LogEventLevel.Warning, logWriteFileModel, logWriteToConsole, logWriteMySqlModel
                );
        }
        public static LoggerConfiguration OdinWrite(this LoggerSinkConfiguration loggerSinkConfiguration, LogEventLevel logLevel, LogWriteFileModel logWriteFileModel, LogWriteToConsoleModel logWriteToConsole, LogWriteMySqlModel logWriteMySqlModel = null)
        {
            return loggerSinkConfiguration.Logger(fileLogger =>
            {
                var config = OdinSerilog.OdinWriteToFile(fileLogger, logLevel, logWriteFileModel);
                if (logWriteToConsole != null)
                {
                    config.WriteTo.Console(
                       outputTemplate:
                            string.IsNullOrEmpty(logWriteToConsole.OutputTemplate)
                            ?
                            "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"
                            :
                            logWriteToConsole.OutputTemplate,
                       theme: logWriteToConsole.ConsoleTheme
                    );
                }
                if (logWriteMySqlModel != null && logWriteMySqlModel.LogLevels != null && logWriteMySqlModel.LogLevels.ToList().Contains((int)logLevel) && logWriteMySqlModel != null)
                {
                    config.WriteTo.MySQL(logWriteMySqlModel.ConnectionString, "tb_logs");
                }
            });
        }
    }
}