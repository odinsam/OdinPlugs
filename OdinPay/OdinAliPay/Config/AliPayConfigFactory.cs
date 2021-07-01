using OdinPlugs.OdinCore.ConfigModel.PayConfigModel;

namespace OdinPlugs.OdinPay.OdinAliPay.Config
{
    public class AliPayConfigFactory
    {
        private static volatile AliPayConfig config;
        private static object syncRoot = new object();
        /// <summary>
        /// 获取wxConfig配置
        /// </summary>
        /// <returns></returns>
        public static AliPayConfig GetConfig(AliPayCnfOptions aliPayConfig)
        {
            if (config == null)
            {
                lock (syncRoot)
                {
                    if (config == null)
                        config = new AliPayConfig(aliPayConfig);
                }
            }
            return config;
        }
    }
}