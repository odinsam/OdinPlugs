using Microsoft.EntityFrameworkCore;
using OdinPlugs.OdinMAF.OdinEF.EFCore.EFExtensions.EFInterface;
using OdinPlugs.OdinMvcCore.OdinInject.InjectInterface;

namespace OdinPlugs.OdinMvcCore.ServicesCore.ServicesInterface.EntityFrameWork
{
    public interface IOdinServiceCore : IService, IAutoInject
    {
        IBaseRepository<T> GetRepository<T>(DbContext _objectContext) where T : class, new();
    }
}