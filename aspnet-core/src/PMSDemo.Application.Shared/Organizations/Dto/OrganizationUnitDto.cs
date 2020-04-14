using Abp.Application.Services.Dto;

namespace PMSDemo.Organizations.Dto
{
    public class OrganizationUnitDto : AuditedEntityDto<long>
    {
        public long? ParentId { get; set; }

        public string Code { get; set; }

        public string DisplayName { get; set; }

        public int MemberCount { get; set; }
        
        public int RoleCount { get; set; }

        public int IndicatorCount { get; set; }

        public int ActivityCount { get; set; }

        public int DeliverableCount { get; set; }
    }
}