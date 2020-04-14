
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PMSDemo.Agencies.Dtos
{
    public class CreateOrEditMdaDto : EntityDto<long?>
    {
		[Required]
		public string DisplayName { get; set; }	

    }
}