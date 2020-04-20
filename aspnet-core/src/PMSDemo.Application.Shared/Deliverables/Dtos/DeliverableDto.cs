
using System;
using Abp.Application.Services.Dto;

namespace PMSDemo.Deliverables.Dtos
{
    public class DeliverableDto : EntityDto<long>
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public long MdaId { get; set; }
        public int PriorityAreaId { get; set; }
        public double? BudgetAmount { get; set; }
        public double? AmountSpent { get; set; }
    }
}