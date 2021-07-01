using Serilog.Events;

namespace OdinPlugs.OdinMAF.OdinSerilog.Models
{
    public class LogWriteMySqlModel
    {
        public int[] LogLevels { get; set; }
        public string ConnectionString { get; set; }
    }
}