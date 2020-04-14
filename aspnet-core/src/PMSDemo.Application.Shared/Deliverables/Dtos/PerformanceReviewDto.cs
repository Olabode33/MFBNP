
using System;
using Abp.Application.Services.Dto;

namespace PMSDemo.Deliverables.Dtos
{
    public class PerformanceReviewDto : EntityDto
    {
        public long OrganizationUnitId { get; set; }
        public string ReviewComment { get; set; }
        public string Challenges { get; set; }
        public string Recommendation { get; set; }
    }
}