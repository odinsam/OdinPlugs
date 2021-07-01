using Serilog.Sinks.SystemConsole.Themes;

namespace OdinPlugs.OdinMAF.OdinSerilog.Models
{
    public class LogWriteToConsoleModel
    {
        public string OutputTemplate { get; set; }
        public SystemConsoleTheme ConsoleTheme { get; set; } = SystemConsoleTheme.Colored;
    }
}