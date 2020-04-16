using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using PMSDemo.Common.Dto;

namespace PMSDemo.PerformanceActivities.Dtos
{
    public class GetPerformanceActivityForEditOutput
    {
		public CreateOrEditPerformanceActivityDto PerformanceActivity { get; set; }
		public string DeliverableName { get; set; }
		public string MdaName { get; set; }
        public List<ActivityAttachmentDto> Attachments { get; set; }
        public AuditInfoDto AuditInfo { get; set; }
    }
}