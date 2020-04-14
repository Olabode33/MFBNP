using PMSDemo.Agencies;
using PMSDemo.PerformanceReporting;
using PMSDemo.PriorityAreas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PMSDemo.Deliverables
{
    [Table("Deliverables")]
    public class Deliverable : PerformanceReportingBase
    {
        public virtual int? PriorityAreaId  {get; set;}

        [ForeignKey("PriorityAreaId")]
        public PriorityArea PriorityAreaFk { get; set; }
    }
}
