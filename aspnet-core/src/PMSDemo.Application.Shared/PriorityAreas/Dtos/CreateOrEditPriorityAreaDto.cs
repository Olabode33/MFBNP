
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PMSDemo.PriorityAreas.Dtos
{
    public class CreateOrEditPriorityAreaDto : EntityDto<int?>
    {

		[Required]
		public string Name { get; set; }
		
		
		public string Description { get; set; }
		
		

    }
}