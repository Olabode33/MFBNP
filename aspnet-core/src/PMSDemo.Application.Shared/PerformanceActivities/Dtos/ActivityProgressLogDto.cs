using PMSDemo.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSDemo.PerformanceActivities.Dtos
{
    public class ActivityProgressLogDto
    {
        public int PerformanceActivityId { get; set; }
        public string Notes { get; set; }
        public string DataSource { get; set; }
        public int OriginalValue { get; set; }
        public int? CompletionLevel { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
