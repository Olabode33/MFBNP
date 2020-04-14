using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PMSDemo.PerformanceActivities.Dtos
{
    public class GetPerformanceActivityForEditOutput
    {
		public CreateOrEditPerformanceActivityDto PerformanceActivity { get; set; }
		public string DeliverableName { get; set; }
		public string MdaName { get; set; }
        public List<ActivityAttachmentDto> Attachments { get; set; }
    }
}