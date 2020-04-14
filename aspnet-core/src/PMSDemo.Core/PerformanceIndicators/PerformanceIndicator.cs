using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using PMSDemo.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PMSDemo.PerformanceIndicators
{
    [Table("PerformanceIndicators")]
    public class PerformanceIndicator: FullAuditedEntity, IMustHaveOrganizationUnit
    {
        public virtual long OrganizationUnitId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string BaselineValue { get; set; }
        public virtual string BaselineComment { get; set; }
        public virtual int Year { get; set; }
        public virtual string Target { get; set; }
        public virtual string Actual { get; set; }
        public virtual DataTypeEnum DataType { get; set; }
        public virtual UnitsEnum Unit { get; set; }
        public virtual ComparisonMethodEnum ComparisonMethod { get; set; }
        public virtual string MeansOfVerification { get; set; }
        public virtual string Note { get; set; }
        public virtual bool CanCascade { get; set; }
    }
}
