using PMSDemo.Common.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSDemo.PerformanceActivities.Dtos
{
    public class ActivityAttachmentDto: AttachmentBaseDto
    {
        public int PerformanceActivityId { get; set; }
    }
}
