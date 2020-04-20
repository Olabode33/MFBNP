
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PMSDemo.Deliverables.Dtos
{
    public class CreateOrEditDeliverableDto : EntityDto<long?>
    {
		[Required]
		public string DisplayName { get; set; }
		public string Description { get; set; }
        [Required]
        public long MdaId { get; set; }
        [Required]
        public int PriorityAreaId { get; set; }

        public double? BudgetAmount { get; set; }
        public double? AmountSpent { get; set; }
    }
}