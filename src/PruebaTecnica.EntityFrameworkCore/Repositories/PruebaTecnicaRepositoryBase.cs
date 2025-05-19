using Abp.Domain.Entities; 
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Repositories;
using PruebaTecnica.EntityFrameworkCore;

namespace PruebaTecnica.Repositories
{
   
    public abstract class PruebaTecnicaRepositoryBase<TEntity, TPrimaryKey>
        : EfCoreRepositoryBase<PruebaTecnicaDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected PruebaTecnicaRepositoryBase(IDbContextProvider<PruebaTecnicaDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        
    }

    
    public abstract class PruebaTecnicaRepositoryBase<TEntity>
        : PruebaTecnicaRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected PruebaTecnicaRepositoryBase(IDbContextProvider<PruebaTecnicaDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
