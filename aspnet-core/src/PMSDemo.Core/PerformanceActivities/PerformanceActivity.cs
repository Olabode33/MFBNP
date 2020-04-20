using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using PMSDemo.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PMSDemo.PerformanceActivities
{
    [Table("PerformanceActivities")]
    public class PerformanceActivity: FullAuditedEntity, IMustHaveOrganizationUnit
    {
        public virtual long OrganizationUnitId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string MilestoneAchieved { get; set; }
        public virtual DateTime? PlannedStartDate { get; set; }
        public virtual DateTime? ActualStartDate { get; set; }
        public virtual DateTime? PlannedCompletionDate { get; set; }
        public virtual DateTime? ActualCompletionDate { get; set; }
        public virtual CompletionStatusEnum CompletionStatus { get; set; }
        public virtual string Note { get; set; }
        public virtual bool CanCascade { get; set; }
        public virtual string DataSource { get; set; }
        public virtual int? CompletionLevel { get; set; }
        public virtual double? BudgetAmount { get; set; }
        public virtual double? AmountSpent { get; set; }
    }
}
