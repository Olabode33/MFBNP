
using System;
using Abp.Application.Services.Dto;

namespace PMSDemo.Agencies.Dtos
{
    public class MdaDto : EntityDto<long>
    {
        public string DisplayName { get; set; }
    }
}