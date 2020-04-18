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
using PMSDemo.PerformanceIndicators;
using PMSDemo.PerformanceActivities;

namespace PMSDemo.Deliverables
{
    [AbpAuthorize(AppPermissions.Pages_Deliverable)]
    public class DeliverablesAppService : PMSDemoAppServiceBase
    {
        private readonly IRepository<Deliverable, long> _deliverableRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<OrganizationUnit, long> _lookup_organizationUnitRepository; 
        private readonly IRepository<PriorityArea> _lookup_priorityAreaRepository; 
        private readonly IRepository<PerformanceIndicator> _lookup_indicatorRepository; 
        private readonly IRepository<PerformanceActivity> _lookup_activityRepository; 
        private readonly OrganizationUnitManager _organizationUnitManager;

        public DeliverablesAppService(
            IRepository<Deliverable, long> deliverableRepository, 
            IRepository<User, long> lookup_userRepository, 
            IRepository<OrganizationUnit, long> lookup_organizationUnitRepository,
            IRepository<PriorityArea> lookup_priorityAreaRepository,
            IRepository<PerformanceIndicator> lookup_indicatorRepository,
            IRepository<PerformanceActivity> lookup_activityRepository,
            OrganizationUnitManager organizationUnitManager)
        {
            _deliverableRepository = deliverableRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_organizationUnitRepository = lookup_organizationUnitRepository;
            _lookup_priorityAreaRepository = lookup_priorityAreaRepository;
            _lookup_indicatorRepository = lookup_indicatorRepository;
            _lookup_activityRepository = lookup_activityRepository;
            _organizationUnitManager = organizationUnitManager;
        }

        public async Task<ListResultDto<GetDeliverableForEditOutput>> GetForPriorityArea(EntityDto input)
        {
            var filteredDeliverables = _deliverableRepository.GetAll()
                                                             .Include(x => x.Parent)
                                                             .Where(x => x.PriorityAreaId == input.Id)
                                                             .Select( x => new GetDeliverableForEditOutput
                                                             {
                                                                 Deliverable = ObjectMapper.Map<CreateOrEditDeliverableDto>(x),
                                                                 MdaName = x.Parent != null ? x.Parent.DisplayName : ""
                                                             });

            var deliverables = await filteredDeliverables.ToListAsync();
            var output = deliverables.Select(x => {
                x.PercentageAchieved = GetDeliverablePercentageAchieved((long)x.Deliverable.Id);
                return x;
            });

            return new ListResultDto<GetDeliverableForEditOutput>()
            {
                Items = output.ToList()
            };
        }

        private double GetDeliverablePercentageAchieved(long deliverableId)
        {
            var indicators = _lookup_indicatorRepository.GetAllList(x => x.OrganizationUnitId == deliverableId);
            var activities = _lookup_activityRepository.GetAllList(x => x.OrganizationUnitId == deliverableId);

            double indicatorPercentageAchieved = 0;
            double activitiesPercentageAchieved = 0;

            if (indicators.Count > 0)
                indicatorPercentageAchieved = indicators.Average(x => x.PercentageAchieved);

            if (activities.Count > 0)
                activitiesPercentageAchieved = activities.Average(x => x.CompletionLevel == null ? 0.0 : (double)x.CompletionLevel);

            return ((indicatorPercentageAchieved + activitiesPercentageAchieved) / 2.00);
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
