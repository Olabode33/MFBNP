using Abp.Domain.Entities.Auditing;
using PMSDemo.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PMSDemo.PerformanceActivities
{
    [Table("ActivityUpdateLogs")]
    public class ActivityUpdateLog: FullAuditedEntity
    {
        public virtual int PerformanceActivityId { get; set; }

        [ForeignKey("PerformanceActivityId")]
        public virtual PerformanceActivity PerformanceIndicatorFk { get; set; }
        public virtual CompletionStatusEnum Status { get; set; }
        public virtual string Notes { get; set; }
    }
}
