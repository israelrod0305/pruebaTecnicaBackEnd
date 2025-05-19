using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace PruebaTecnica
{
    [DependsOn(
        typeof(PruebaTecnicaCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class PruebaTecnicaApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PruebaTecnicaApplicationModule).GetAssembly());
        }
    }
}