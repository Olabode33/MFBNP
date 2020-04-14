using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using PMSDemo.Configure;
using PMSDemo.Startup;
using PMSDemo.Test.Base;

namespace PMSDemo.GraphQL.Tests
{
    [DependsOn(
        typeof(PMSDemoGraphQLModule),
        typeof(PMSDemoTestBaseModule))]
    public class PMSDemoGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PMSDemoGraphQLTestModule).GetAssembly());
        }
    }
}