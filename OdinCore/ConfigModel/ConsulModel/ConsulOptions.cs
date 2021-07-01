namespace OdinPlugs.OdinCore.ConfigModel.ConsulModel
{
    public class CheckOption
    {
        public int DeregisterCriticalServiceAfter { get; set; }
        public string HealthApi { get; set; }
        public int Interval { get; set; }
        public int Timeout { get; set; }
    }
    public class ConsulOptions
    {
        public bool Enable { get; set; }
        public string Protocol { get; set; }
        public string ConsulName { get; set; }
        public string ConsulIpAddress { get; set; }
        public int ConsulPort { get; set; }
        public string DataCenter { get; set; }
        /// <summary>
        /// 服务器权重
        /// </summary>
        /// <value></value>
        public int Weight { get; set; } = 50;
        public CheckOption ConsulCheck { get; set; }

    }
}