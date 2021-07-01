using System;
using Microsoft.EntityFrameworkCore;
using OdinPlugs.OdinMAF.OdinEF.EFCore.EFExtensions;
using OdinPlugs.OdinMAF.OdinEF.EFCore.EFExtensions.EFInterface;

namespace OdinPlugs.OdinMAF.OdinEF.EFCore
{
    public class DBContextFactory
    {
        public static IBaseRepository<T> GetRepository<T>(DbContext _objectContext) where T : class, new()
        {
            return new BaseRepository<T>(_objectContext);
        }
    }
}