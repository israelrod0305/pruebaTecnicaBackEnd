using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using PruebaTecnica.Configuration;
using PruebaTecnica.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;

namespace PruebaTecnica.Web.Startup
{
    [DependsOn(
        typeof(PruebaTecnicaApplicationModule), 
        typeof(PruebaTecnicaEntityFrameworkCoreModule), 
        typeof(AbpAspNetCoreModule))]
    public class PruebaTecnicaWebModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public PruebaTecnicaWebModule(IWebHostEnvironment env)
        {
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(PruebaTecnicaConsts.ConnectionStringName);

            Configuration.Navigation.Providers.Add<PruebaTecnicaNavigationProvider>();

            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(PruebaTecnicaApplicationModule).GetAssembly()
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PruebaTecnicaWebModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(PruebaTecnicaWebModule).Assembly);
        }
    }
}
