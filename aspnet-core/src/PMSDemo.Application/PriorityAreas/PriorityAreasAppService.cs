

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using PMSDemo.PriorityAreas.Dtos;
using PMSDemo.Dto;
using Abp.Application.Services.Dto;
using PMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace PMSDemo.PriorityAreas
{
	[AbpAuthorize(AppPermissions.Pages_PriorityAreas)]
    public class PriorityAreasAppService : PMSDemoAppServiceBase, IPriorityAreasAppService
    {
		 private readonly IRepository<PriorityArea> _priorityAreaRepository;
		 

		  public PriorityAreasAppService(IRepository<PriorityArea> priorityAreaRepository ) 
		  {
			_priorityAreaRepository = priorityAreaRepository;
			
		  }

		 public async Task<PagedResultDto<GetPriorityAreaForViewDto>> GetAll(GetAllPriorityAreasInput input)
         {
			
			var filteredPriorityAreas = _priorityAreaRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter));

			var pagedAndFilteredPriorityAreas = filteredPriorityAreas
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var priorityAreas = from o in pagedAndFilteredPriorityAreas
                         select new GetPriorityAreaForViewDto() {
							PriorityArea = new PriorityAreaDto
							{
                                Name = o.Name,
                                Id = o.Id
							}
						};

            var totalCount = await filteredPriorityAreas.CountAsync();

            return new PagedResultDto<GetPriorityAreaForViewDto>(
                totalCount,
                await priorityAreas.ToListAsync()
            );
         }
		 

		 public async Task<GetPriorityAreaForEditOutput> GetPriorityAreaForEdit(EntityDto input)
         {
            var priorityArea = await _priorityAreaRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPriorityAreaForEditOutput {PriorityArea = ObjectMapper.Map<CreateOrEditPriorityAreaDto>(priorityArea)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPriorityAreaDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_PriorityAreas_Create)]
		 protected virtual async Task Create(CreateOrEditPriorityAreaDto input)
         {
            var priorityArea = ObjectMapper.Map<PriorityArea>(input);

			

            await _priorityAreaRepository.InsertAsync(priorityArea);
         }

		 [AbpAuthorize(AppPermissions.Pages_PriorityAreas_Edit)]
		 protected virtual async Task Update(CreateOrEditPriorityAreaDto input)
         {
            var priorityArea = await _priorityAreaRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, priorityArea);
         }

		 [AbpAuthorize(AppPermissions.Pages_PriorityAreas_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _priorityAreaRepository.DeleteAsync(input.Id);
         } 
    }
}