using Abp.Domain.Entities.Auditing;
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
        public virtual string Target { get; set; }
        public virtual string Actual { get; set; }
        public virtual string DataSource { get; set; }
        public virtual string Note { get; set; }
    }
}
