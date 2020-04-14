using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PMSDemo.Deliverables.Dtos
{
    public class GetPerformanceReviewForEditOutput
    {
		public CreateOrEditPerformanceReviewDto Review { get; set; }
		public string DeliverableName { get; set; }
		public string MdaName { get; set; }
    }
}