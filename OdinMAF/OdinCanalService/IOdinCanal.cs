using OdinPlugs.OdinMvcCore.OdinInject.InjectInterface;
using OdinPlugs.OdinMAF.OdinCanalService.OdinCanalModels;

namespace OdinPlugs.OdinMAF.OdinCanalService
{
    public interface IOdinCanal : IAutoInject
    {
        OdinCanalModel GetCanalInfo(string jsonData);
    }
}