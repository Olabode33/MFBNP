using Microsoft.Extensions.Configuration;

namespace PMSDemo.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
