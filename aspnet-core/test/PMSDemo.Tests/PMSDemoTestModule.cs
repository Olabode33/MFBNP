using Abp.Modules;
using PMSDemo.Test.Base;

namespace PMSDemo.Tests
{
    [DependsOn(typeof(PMSDemoTestBaseModule))]
    public class PMSDemoTestModule : AbpModule
    {
       
    }
}
