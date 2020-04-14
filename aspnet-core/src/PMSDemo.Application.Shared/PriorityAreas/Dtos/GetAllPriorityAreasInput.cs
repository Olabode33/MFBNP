using Abp.Application.Services.Dto;
using System;

namespace PMSDemo.PriorityAreas.Dtos
{
    public class GetAllPriorityAreasInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }



    }
}