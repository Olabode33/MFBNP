using System;
using System.Collections.Generic;
using System.Text;

namespace PMSDemo.PerformanceActivities.Dtos
{
    public class UpdateActivityProgressDto
    {
        public CreateOrEditPerformanceActivityDto Activity { get; set; }
        public List<ActivityAttachmentDto> Attachments { get; set; }
    }
}
