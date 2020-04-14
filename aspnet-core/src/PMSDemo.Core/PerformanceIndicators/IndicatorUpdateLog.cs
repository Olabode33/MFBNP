using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PMSDemo.PerformanceIndicators
{
    [Table("IndicatorUpdateLogs")]
    public class IndicatorUpdateLog: FullAuditedEntity
    {
        public virtual int PerformanceIndicatorId { get; set; }

        [ForeignKey("PerformanceIndicatorId")]
        public virtual PerformanceIndicator PerformanceIndicatorFk { get; set; }
        public virtual string ActualValue { get; set; }
        public virtual string Note { get; set; }
    }
}
