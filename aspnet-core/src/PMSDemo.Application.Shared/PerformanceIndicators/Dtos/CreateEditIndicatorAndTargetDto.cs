using System;
using System.Collections.Generic;
using System.Text;

namespace PMSDemo.PerformanceIndicators.Dtos
{
    public class CreateEditIndicatorAndTargetDto
    {
        public CreateOrEditPerformanceIndicatorDto Indicator { get; set; }
        public List<IndicatorYearlyTargetDto> YearlyTarget { get; set; }
    }
}
