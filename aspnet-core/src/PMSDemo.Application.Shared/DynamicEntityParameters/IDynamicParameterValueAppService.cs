﻿using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using PMSDemo.DynamicEntityParameters.Dto;

namespace PMSDemo.DynamicEntityParameters
{
    public interface IDynamicParameterValueAppService
    {
        Task<DynamicParameterValueDto> Get(int id);

        Task<ListResultDto<DynamicParameterValueDto>> GetAllValuesOfDynamicParameter(EntityDto input);

        Task Add(DynamicParameterValueDto dto);

        Task Update(DynamicParameterValueDto dto);

        Task Delete(int id);
    }
}
