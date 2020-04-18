using Abp.Domain.Entities.Auditing;
using PMSDemo.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PMSDemo.PerformanceIndicators
{
    [Table("IndicatorYearlyTargets")]
    public class IndicatorYearlyTarget : FullAuditedEntity
    {
        public virtual int IndicatorId { get; set; }
        public virtual int Year { get; set; }
        public virtual string Description { get; set; }
        public virtual ComparisonMethodEnum ComparisonMethod { get; set; }
        public virtual string MeansOfVerification { get; set; }
        public virtual string Target { get; set; }
        public virtual string Actual { get; set; }
        public virtual string DataSource { get; set; }
        public virtual string Note { get; set; }
        public virtual int PercentageAchieved { get; set; }

    }
}
