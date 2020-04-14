using Abp.Modules;
using Abp.Reflection.Extensions;

namespace PMSDemo
{
    public class PMSDemoCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PMSDemoCoreSharedModule).GetAssembly());
        }
    }
}