using PMSDemo.PerformanceReporting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PMSDemo.PerformanceActivities
{
    [Table("ActivityAttachments")]
    public class ActivityAttachment: AttachmentBase
    {
        public virtual int PerformanceActivityId { get; set; }

        [ForeignKey("PerformanceActivityId")]
        public PerformanceActivity PerformanceActivityFk { get; set; }
    }
}
