using PMSDemo.PerformanceReporting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PMSDemo.PerformanceIndicators
{
    [Table("IndicatorAttachments")]
    public class IndicatorAttachment: AttachmentBase
    {
        public virtual int IndicatorTargetId { get; set; }

        [ForeignKey("IndicatorTargetId")]
        public IndicatorYearlyTarget IndicatorTargetFk { get; set; }
    }
}
