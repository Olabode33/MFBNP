using PMSDemo.Common.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSDemo.PerformanceIndicators.Dtos
{
    public class IndicatorAttachmentDto: AttachmentBaseDto
    {
        public int IndicatorTargetId { get; set; }
    }
}
