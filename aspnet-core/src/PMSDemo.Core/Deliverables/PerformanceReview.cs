using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PMSDemo.Deliverables
{
    [Table("PerformanceReviews")]
    public class PerformanceReview: FullAuditedEntity, IMustHaveOrganizationUnit
    {
        public virtual long OrganizationUnitId { get; set; }
        public virtual string ReviewComment { get; set; }
        public virtual string Challenges { get; set; }
        public virtual string Recommendation { get; set; }
    }
}
