
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PMSDemo.Deliverables.Dtos
{
    public class CreateOrEditPerformanceReviewDto : EntityDto<long?>
    {
		[Required]
        public long OrganizationUnitId { get; set; }
        public string ReviewComment { get; set; }
        public string Challenges { get; set; }
        public string Recommendation { get; set; }

    }
}