using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PMSDemo.PerformanceActivities.Dtos
{
    public class GetAllPerformanceActivityInput : PagedAndSortedResultRequestDto
    {
		public string FilterText { get; set; }
        public long OrganizationUnitId { get; set; }
    }
}