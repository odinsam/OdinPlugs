using System;
using Serilog;
using Serilog.Events;
using OdinPlugs.OdinMAF.OdinSerilog.Models;

namespace OdinPlugs.OdinMAF.OdinSerilog
{
    public class OdinSerilog
    {

        public static LoggerConfiguration OdinWriteToFile(LoggerConfiguration fileLogger, LogEventLevel logLevel, LogWriteFileModel logWriteModel)
        {

            return fileLogger.Filter
                   .ByIncludingOnly(p => p.Level.Equals(logLevel))
                   .WriteTo.File(
                       path:
                        string.IsNullOrEmpty(logWriteModel.FileName) ?
                            $"logs/{DateTime.Now.ToString("yyyyMMdd")}/log-{DateTime.Now.ToString("yyyyMMdd")}-{logLevel.ToString()}.txt"
                            :
                            logWriteModel.FileName,
                       fileSizeLimitBytes: logWriteModel.FileSizeLimitBytes,
                       rollOnFileSizeLimit: logWriteModel.RollOnFileSizeLimit,
                       shared: logWriteModel.Shared,
                       flushToDiskInterval: logWriteModel.FlushToDiskInterval
                   );
        }
    }
}