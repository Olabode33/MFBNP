using Abp.Organizations;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSDemo.PerformanceReporting
{
    public class PerformanceReportingBase: OrganizationUnit
    {
        public virtual string Description { get; set; }
    }
}
