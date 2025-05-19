using Abp.Dependency;
using Abp.EntityFrameworkCore;
using Abp.Modules;
using Abp.Reflection.Extensions;
using PruebaTecnica.Core;

namespace PruebaTecnica.EntityFrameworkCore
{
    [DependsOn(
        typeof(PruebaTecnicaCoreModule), 
        typeof(AbpEntityFrameworkCoreModule))]
    public class PruebaTecnicaEntityFrameworkCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PruebaTecnicaEntityFrameworkCoreModule).GetAssembly());

            
        }
    }
}