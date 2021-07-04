using OdinPlugs.OdinMvcCore.OdinInject;
using OdinPlugs.OdinNetCore.OdinSnowFlake.SnowFlakeInterface;

namespace OdinPlugs.OdinNetCore.OdinSnowFlake.Utils
{
    public static class OdinSnowFlakeHelper
    {
        public static long CreateSnowFlake()
        {
            return OdinInjectHelper.GetService<IOdinSnowFlake>().NextId();
        }
    }
}