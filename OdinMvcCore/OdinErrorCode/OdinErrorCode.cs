using OdinPlugs.OdinCore.Models.ErrorCode;
using OdinPlugs.OdinInject;
using OdinPlugs.OdinMAF.OdinCacheManager;

namespace OdinPlugs.OdinMvcCore.OdinErrorCode
{
    public class OdinErrorCode : IOdinErrorCode
    {
        private readonly IOdinCacheManager odinCacheManager;

        public OdinErrorCode()
        {
            this.odinCacheManager = OdinInjectCore.GetService<IOdinCacheManager>();
        }
        public ErrorCode_Model GetErrorModel(string code)
        {
            return this.odinCacheManager.Get<ErrorCode_Model>(code);
        }
    }
}