using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace PMSDemo
{
    [DependsOn(typeof(PMSDemoClientModule), typeof(AbpAutoMapperModule))]
    public class PMSDemoXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PMSDemoXamarinSharedModule).GetAssembly());
        }
    }
}