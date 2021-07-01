using OdinPlugs.OdinCore.Models.ErrorCode;
using OdinPlugs.OdinMAF.OdinCacheManager;
using OdinPlugs.OdinMvcCore.OdinInject;

namespace OdinPlugs.OdinMvcCore.OdinErrorCode
{
    public class OdinErrorCode : IOdinErrorCode
    {
        private readonly IOdinCacheManager odinCacheManager;

        public OdinErrorCode()
        {
            this.odinCacheManager = OdinInjectHelper.GetService<IOdinCacheManager>();
        }
        public ErrorCode_Model GetErrorModel(string code)
        {
            return this.odinCacheManager.Get<ErrorCode_Model>(code);
        }
    }
}