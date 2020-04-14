using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSDemo.Common.Dto
{
    public class AttachmentBaseDto: EntityDto
    {
        public string FileName { get; set; }
        public Guid? DocumentId { get; set; }
        public string FileFormat { get; set; }
    }
}
