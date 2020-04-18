using Abp.Application.Services.Dto;
using PMSDemo.PerformanceActivities.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMSDemo.PerformanceActivities
{
    public interface IPerformanceActivitiesAppService
    {
        Task CreateOrEdit(CreateOrEditPerformanceActivityDto input);
        Task Delete(EntityDto input);
        Task<PagedResultDto<GetPerformanceActivityForEditOutput>> GetAllForUnit(GetAllPerformanceActivityInput input);
        Task<GetPerformanceActivityForEditOutput> GetPerformanceActivityForEdit(EntityDto input);
        Task<List<ActivityProgressLogDto>> GetTargetUpdateLog(EntityDto input);
        Task UpdateProgress(UpdateActivityProgressDto input);
    }
}