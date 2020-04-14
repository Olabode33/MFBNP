using Abp.Application.Services.Dto;

namespace PMSDemo.DynamicEntityParameters.Dto
{
    public class EntityDynamicParameterDto : EntityDto
    {
        public string EntityFullName { get; set; }

        public string DynamicParameterName { get; set; }

        public int DynamicParameterId { get; set; }

        public int? TenantId { get; set; }
    }
}
