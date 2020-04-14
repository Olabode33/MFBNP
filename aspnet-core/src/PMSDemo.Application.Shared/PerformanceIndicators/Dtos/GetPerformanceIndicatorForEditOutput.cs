using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PMSDemo.PerformanceIndicators.Dtos
{
    public class GetPerformanceIndicatorForEditOutput
    {
		public CreateOrEditPerformanceIndicatorDto PerformanceIndicator { get; set; }
		public string DeliverableName { get; set; }
		public string MdaName { get; set; }
        public bool Inherited { get; set; }
        public List<UpdateTargetDto> Targets { get; set; }
    }
}