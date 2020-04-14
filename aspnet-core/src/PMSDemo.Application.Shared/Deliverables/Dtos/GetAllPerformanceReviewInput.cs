using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PMSDemo.Deliverables.Dtos
{
    public class GetAllPerformanceReviewInput : PagedAndSortedResultRequestDto
    {
		public string FilterText { get; set; }
        public long OrganizationUnitId { get; set; }
    }
}