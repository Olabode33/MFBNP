using Abp.Application.Services.Dto;
using PMSDemo.PerformanceIndicators.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMSDemo.PerformanceIndicators
{
    public interface IPerformanceIndicatorsAppService
    {
        Task CreateOrEdit(CreateEditIndicatorAndTargetDto input);
        Task Delete(EntityDto input);
        Task<PagedResultDto<GetPerformanceIndicatorForEditOutput>> GetAllForUnit(GetAllPerformanceIndicatorsInput input);
        Task<GetPerformanceIndicatorForEditOutput> GetPerformanceIndicatorForEdit(EntityDto input);
        Task<List<TargetProgressLogDto>> GetTargetUpdateLog(EntityDto input);
        Task UpdateProgress(CreateOrEditPerformanceIndicatorDto input);
        Task UpdateYearTargetProgress(UpdateTargetDto input);
    }
}