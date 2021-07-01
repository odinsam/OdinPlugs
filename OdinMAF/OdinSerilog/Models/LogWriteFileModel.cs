using System;
using Serilog.Events;

namespace OdinPlugs.OdinMAF.OdinSerilog.Models
{
    public class LogWriteFileModel
    {
        public string FileName { get; set; }
        public int FileSizeLimitBytes { get; set; } = 1000000;
        public bool RollOnFileSizeLimit { get; set; } = true;
        public bool Shared { get; set; } = true;
        public TimeSpan FlushToDiskInterval { get; set; } = TimeSpan.FromSeconds(1);

    }
}