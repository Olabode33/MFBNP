using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using PMSDemo.PriorityAreas.Dtos;
using PMSDemo.Dto;


namespace PMSDemo.PriorityAreas
{
    public interface IPriorityAreasAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPriorityAreaForViewDto>> GetAll(GetAllPriorityAreasInput input);

		Task<GetPriorityAreaForEditOutput> GetPriorityAreaForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditPriorityAreaDto input);

		Task Delete(EntityDto input);

		
    }
}