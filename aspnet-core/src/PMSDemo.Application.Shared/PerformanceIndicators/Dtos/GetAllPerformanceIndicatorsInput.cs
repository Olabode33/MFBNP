using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PMSDemo.PerformanceIndicators.Dtos
{
    public class GetAllPerformanceIndicatorsInput : PagedAndSortedResultRequestDto
    {
		public string FilterText { get; set; }
        public long OrganizationUnitId { get; set; }
    }
}