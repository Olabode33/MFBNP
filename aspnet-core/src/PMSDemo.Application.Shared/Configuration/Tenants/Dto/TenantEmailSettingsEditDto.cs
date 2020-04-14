using Abp.Auditing;
using PMSDemo.Configuration.Dto;

namespace PMSDemo.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}