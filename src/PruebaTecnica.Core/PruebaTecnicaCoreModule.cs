using Abp.Modules;
using Abp.Reflection.Extensions;
using PruebaTecnica.Localization;

namespace PruebaTecnica
{
    public class PruebaTecnicaCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            PruebaTecnicaLocalizationConfigurer.Configure(Configuration.Localization);
            
            Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase = PruebaTecnicaConsts.DefaultPassPhrase;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PruebaTecnicaCoreModule).GetAssembly());
        }
    }
}