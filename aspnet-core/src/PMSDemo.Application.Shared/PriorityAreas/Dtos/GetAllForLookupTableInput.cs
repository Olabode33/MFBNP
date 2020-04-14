using Abp.Application.Services.Dto;

namespace PMSDemo.PriorityAreas.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}