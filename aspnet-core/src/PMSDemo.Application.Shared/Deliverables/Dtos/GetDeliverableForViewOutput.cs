using Abp.Application.Services.Dto;
using PMSDemo.PerformanceActivities.Dtos;
using PMSDemo.PerformanceIndicators.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSDemo.Deliverables.Dtos
{
    public class GetDeliverableForViewOutput
    {
        public ListResultDto<GetDeliverableForEditOutput> Deliverables { get; set; }
        public ListResultDto<GetPerformanceIndicatorForEditOutput> Indicators { get; set; }
        public ListResultDto<GetPerformanceActivityForEditOutput> Activities { get; set; }
    }
}
