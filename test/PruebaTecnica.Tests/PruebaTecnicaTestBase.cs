using System;
using System.Threading.Tasks;
using Abp.TestBase;
using PruebaTecnica.EntityFrameworkCore;
using PruebaTecnica.Tests.TestDatas;

namespace PruebaTecnica.Tests
{
    public class PruebaTecnicaTestBase : AbpIntegratedTestBase<PruebaTecnicaTestModule>
    {
        public PruebaTecnicaTestBase()
        {
            UsingDbContext(context => new TestDataBuilder(context).Build());
        }

        protected virtual void UsingDbContext(Action<PruebaTecnicaDbContext> action)
        {
            using (var context = LocalIocManager.Resolve<PruebaTecnicaDbContext>())
            {
                action(context);
                context.SaveChanges();
            }
        }

        protected virtual T UsingDbContext<T>(Func<PruebaTecnicaDbContext, T> func)
        {
            T result;

            using (var context = LocalIocManager.Resolve<PruebaTecnicaDbContext>())
            {
                result = func(context);
                context.SaveChanges();
            }

            return result;
        }

        protected virtual async Task UsingDbContextAsync(Func<PruebaTecnicaDbContext, Task> action)
        {
            using (var context = LocalIocManager.Resolve<PruebaTecnicaDbContext>())
            {
                await action(context);
                await context.SaveChangesAsync(true);
            }
        }

        protected virtual async Task<T> UsingDbContextAsync<T>(Func<PruebaTecnicaDbContext, Task<T>> func)
        {
            T result;

            using (var context = LocalIocManager.Resolve<PruebaTecnicaDbContext>())
            {
                result = await func(context);
                context.SaveChanges();
            }

            return result;
        }
    }
}
