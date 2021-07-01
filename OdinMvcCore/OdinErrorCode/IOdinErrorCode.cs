using OdinPlugs.OdinCore.Models.ErrorCode;
using OdinPlugs.OdinMvcCore.OdinInject.InjectInterface;

namespace OdinPlugs.OdinMvcCore.OdinErrorCode
{
    public interface IOdinErrorCode : IAutoInject
    {
        ErrorCode_Model GetErrorModel(string code);
    }
}