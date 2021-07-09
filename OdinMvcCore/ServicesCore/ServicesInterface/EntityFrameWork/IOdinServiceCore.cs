using Microsoft.EntityFrameworkCore;
using OdinPlugs.OdinInject.InjectInterface;
using OdinPlugs.OdinMAF.OdinEF.EFCore.EFExtensions.EFInterface;

namespace OdinPlugs.OdinMvcCore.ServicesCore.ServicesInterface.EntityFrameWork
{
    public interface IOdinServiceCore : IService, IAutoInject
    {
        IBaseRepository<T> GetRepository<T>(DbContext _objectContext) where T : class, new();
    }
}