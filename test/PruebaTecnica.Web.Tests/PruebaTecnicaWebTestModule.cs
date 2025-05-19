using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using PruebaTecnica.Web.Startup;
namespace PruebaTecnica.Web.Tests
{
    [DependsOn(
        typeof(PruebaTecnicaWebModule),
        typeof(AbpAspNetCoreTestBaseModule)
        )]
    public class PruebaTecnicaWebTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PruebaTecnicaWebTestModule).GetAssembly());
        }
    }
}