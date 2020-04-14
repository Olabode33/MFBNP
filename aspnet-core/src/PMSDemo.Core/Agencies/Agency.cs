using Abp.Organizations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PMSDemo.Agencies
{
    [Table("Mdas")]
    public class Mda: OrganizationUnit
    {
    }
}
