using Abp.Domain.Entities.Auditing;
using PMSDemo.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSDemo.PerformanceReporting
{
    public class AttachmentBase: FullAuditedEntity
    {
		public virtual string FileName { get; set; }
		public virtual Guid? DocumentId { get; set; }
		public BinaryObject Document { get; set; }
		public virtual string FileFormat { get; set; }
	}
}
