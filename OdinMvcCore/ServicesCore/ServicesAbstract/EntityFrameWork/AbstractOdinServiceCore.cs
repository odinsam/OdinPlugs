using Microsoft.EntityFrameworkCore;
using OdinPlugs.OdinMAF.OdinEF.EFCore;
using OdinPlugs.OdinMAF.OdinEF.EFCore.EFExtensions.EFInterface;
using OdinPlugs.OdinMvcCore.ServicesCore.ServicesInterface.EntityFrameWork;

namespace OdinPlugs.OdinMvcCore.ServicesCore.ServicesAbstract.EntityFrameWork
{
    public abstract class AbstractOdinServiceCore : IOdinServiceCore
    {
        public virtual IBaseRepository<T> GetRepository<T>(DbContext _objectContext) where T : class, new()
        {
            return DBContextFactory.GetRepository<T>(_objectContext);
        }
    }
}