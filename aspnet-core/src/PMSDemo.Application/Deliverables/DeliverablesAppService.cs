using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using PMSDemo.Dto;
using Abp.Application.Services.Dto;
using PMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using PMSDemo.Authorization.Users;
using Abp.Organizations;
using PMSDemo.Deliverables.Dtos;
using PMSDemo.PriorityAreas;

namespace PMSDemo.Deliverables
{
    [AbpAuthorize(AppPermissions.Pages_Deliverable)]
    public class DeliverablesAppService : PMSDemoAppServiceBase
    {
        private readonly IRepository<Deliverable, long> _deliverableRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<OrganizationUnit, long> _lookup_organizationUnitRepository; 
        private readonly IRepository<PriorityArea> _lookup_priorityAreaRepository; 
        private readonly OrganizationUnitManager _organizationUnitManager;

        public DeliverablesAppService(
            IRepository<Deliverable, long> deliverableRepository, 
            IRepository<User, long> lookup_userRepository, 
            IRepository<OrganizationUnit, long> lookup_organizationUnitRepository,
            IRepository<PriorityArea> lookup_priorityAreaRepository,
            OrganizationUnitManager organizationUnitManager)
        {
            _deliverableRepository = deliverableRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_organizationUnitRepository = lookup_organizationUnitRepository;
            _lookup_priorityAreaRepository = lookup_priorityAreaRepository;
            _organizationUnitManager = organizationUnitManager;
        }

        [AbpAuthorize(AppPermissions.Pages_Deliverable_Edit)]
        public async Task<GetDeliverableForEditOutput> GetDeliverableForEdit(EntityDto<long> input)
        {
            var deliverable = await _deliverableRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDeliverableForEditOutput { Deliverable = ObjectMapper.Map<CreateOrEditDeliverableDto>(deliverable) };

            if (deliverable.PriorityAreaId != null)
            {
                var priorityArea = await _lookup_priorityAreaRepository.FirstOrDefaultAsync((int)deliverable.PriorityAreaId);
                output.PriorityAreaName = priorityArea.Name;
            }

            if (deliverable.ParentId != null)
            {
                var mda = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((int)deliverable.ParentId);
                output.MdaName = mda.DisplayName;
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDeliverableDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Deliverable_Create)]
        protected virtual async Task Create(CreateOrEditDeliverableDto input)
        {
            var deliverable = ObjectMapper.Map<Deliverable>(input);

            await _organizationUnitManager.CreateAsync(deliverable);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Deliverable_Edit)]
        protected virtual async Task Update(CreateOrEditDeliverableDto input)
        {
            var deliverable = await _deliverableRepository.FirstOrDefaultAsync((int)input.Id);
            var mapped = ObjectMapper.Map(input, deliverable);
            await _organizationUnitManager.UpdateAsync(mapped);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Deliverable_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _deliverableRepository.DeleteAsync(input.Id);
        }


    }
}
