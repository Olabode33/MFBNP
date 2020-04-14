using System;
using System.Collections.Generic;
using System.Text;

namespace PMSDemo.PerformanceIndicators.Dtos
{
    public class UpdateTargetDto
    {
        public IndicatorYearlyTargetDto Target { get; set; }
        public List<IndicatorAttachmentDto> Attachments { get; set; }
    }
}
