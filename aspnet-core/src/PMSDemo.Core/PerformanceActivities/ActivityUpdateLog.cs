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
        public virtual string DataSource { get; set; }
        public virtual int OriginalValue { get; set; }
        public virtual int? CompletionLevel { get; set; }
        public virtual double? BudgetAmount { get; set; }
        public virtual double? AmountSpent { get; set; }
    }
}
