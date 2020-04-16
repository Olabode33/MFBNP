using System;
using System.Collections.Generic;
using System.Text;

namespace PMSDemo.Common.Dto
{
    public class AuditInfoDto
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}
