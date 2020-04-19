using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSDemo.Agencies.Dtos
{
    public class GetAllMdaInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
