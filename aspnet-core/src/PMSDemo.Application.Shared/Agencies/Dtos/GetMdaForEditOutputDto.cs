using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PMSDemo.Agencies.Dtos
{
    public class GetMdaForEditOutput
    {
		public CreateOrEditMdaDto Mda { get; set; }
        public string ResponsiblePersonName { get; set; }
    }
}