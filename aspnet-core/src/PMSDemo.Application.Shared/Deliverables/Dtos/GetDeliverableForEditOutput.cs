using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PMSDemo.Deliverables.Dtos
{
    public class GetDeliverableForEditOutput
    {
		public CreateOrEditDeliverableDto Deliverable { get; set; }
		public string PriorityAreaName { get; set; }
		public string MdaName { get; set; }
    }
}