using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace PMSDemo.PriorityAreas
{
	[Table("PriorityAreas")]
    public class PriorityArea : FullAuditedEntity 
    {

		[Required]
		public virtual string Name { get; set; }
		
		public virtual string Description { get; set; }
		

    }
}