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
using PMSDemo.PerformanceIndicators.Dtos;
using PMSDemo.PerformanceActivities.Dtos;

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
        private readonly IRepository<PerformanceReview> _lookup_reviewRepository; 
        private readonly OrganizationUnitManager _organizationUnitManager;

        public DeliverablesAppService(
            IRepository<Deliverable, long> deliverableRepository, 
            IRepository<User, long> lookup_userRepository, 
            IRepository<OrganizationUnit, long> lookup_organizationUnitRepository,
            IRepository<PriorityArea> lookup_priorityAreaRepository,
            IRepository<PerformanceIndicator> lookup_indicatorRepository,
            IRepository<PerformanceActivity> lookup_activityRepository,
            IRepository<PerformanceReview> lookup_reviewRepository,
            OrganizationUnitManager organizationUnitManager)
        {
            _deliverableRepository = deliverableRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_organizationUnitRepository = lookup_organizationUnitRepository;
            _lookup_priorityAreaRepository = lookup_priorityAreaRepository;
            _lookup_indicatorRepository = lookup_indicatorRepository;
            _lookup_activityRepository = lookup_activityRepository;
            _lookup_reviewRepository = lookup_reviewRepository;
            _organizationUnitManager = organizationUnitManager;
        }

        public async Task<GetDeliverableForViewOutput> GetForPriorityArea(EntityDto input)
        {
            var filteredDeliverables = _deliverableRepository.GetAll()
                                                             .Include(x => x.Parent)
                                                             .Where(x => x.PriorityAreaId == input.Id)
                                                             .Select( x => new GetDeliverableForEditOutput
                                                             {
                                                                 Deliverable = ObjectMapper.Map<CreateOrEditDeliverableDto>(x),
                                                                 MdaName = x.Parent != null ? x.Parent.DisplayName : ""
                                                             });

            var deliverables = await filteredDeliverables.OrderBy(x => x.MdaName).ToListAsync();

            var output = deliverables.Select(x => {
                x.PercentageAchieved = GetDeliverablePercentageAchieved((long)x.Deliverable.Id);
                return x;
            });

            List<GetPerformanceIndicatorForEditOutput> indicators = new List<GetPerformanceIndicatorForEditOutput>();
            List<GetPerformanceActivityForEditOutput> activites = new List<GetPerformanceActivityForEditOutput>();
            List<GetPerformanceReviewForEditOutput> reviews = new List<GetPerformanceReviewForEditOutput>();

            foreach (var item in deliverables)
            {
                var deliverableIndicators = await GetInidcatorsForDeliverable((long)item.Deliverable.Id);
                var deliverableActivities = await GetActivitiesForDeliverable((long)item.Deliverable.Id);
                var deliverableReviews = await GetReviewsForDeliverable((long)item.Deliverable.Id);
                indicators.AddRange(deliverableIndicators);
                activites.AddRange(deliverableActivities);
                reviews.AddRange(deliverableReviews);
            }

            return new GetDeliverableForViewOutput()
            {
                Deliverables = new ListResultDto<GetDeliverableForEditOutput>() { Items = output.ToList() },
                Indicators = new ListResultDto<GetPerformanceIndicatorForEditOutput>() { Items = indicators },
                Activities = new ListResultDto<GetPerformanceActivityForEditOutput>() { Items = activites },
                Reviews = new ListResultDto<GetPerformanceReviewForEditOutput>() { Items = reviews }
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

        private async Task<List<GetPerformanceIndicatorForEditOutput>> GetInidcatorsForDeliverable(long deliverableId)
        {

            var filteredIndicator = _lookup_indicatorRepository.GetAll().Where(x => x.OrganizationUnitId == deliverableId);

            var indicators = from o in filteredIndicator
                             join ou in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals ou.Id into ou1
                             from ou2 in ou1.DefaultIfEmpty()

                             join m in _lookup_organizationUnitRepository.GetAll() on ou2.ParentId equals m.Id into m1
                             from m2 in m1.DefaultIfEmpty()

                             select new GetPerformanceIndicatorForEditOutput()
                             {
                                 PerformanceIndicator = ObjectMapper.Map<CreateOrEditPerformanceIndicatorDto>(o),
                                 DeliverableName = ou2 == null ? "" : ou2.DisplayName,
                                 MdaName = m2 == null ? "" : m2.DisplayName,
                             };

            var totalCount = await indicators.CountAsync();

            return await indicators.ToListAsync();
        }

        private async Task<List<GetPerformanceActivityForEditOutput>> GetActivitiesForDeliverable(long deliverableId)
        {

            var filteredIndicator = _lookup_activityRepository.GetAll()
                                                              .Where(x => x.OrganizationUnitId == deliverableId);

            var activities = from o in filteredIndicator
                             join ou in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals ou.Id into ou1
                             from ou2 in ou1.DefaultIfEmpty()

                             join m in _lookup_organizationUnitRepository.GetAll() on ou2.ParentId equals m.Id into m1
                             from m2 in m1.DefaultIfEmpty()

                             select new GetPerformanceActivityForEditOutput()
                             {
                                 PerformanceActivity = ObjectMapper.Map<CreateOrEditPerformanceActivityDto>(o),
                                 DeliverableName = ou2 == null ? "" : ou2.DisplayName,
                                 MdaName = m2 == null ? "" : m2.DisplayName
                             };

            return await activities.ToListAsync();
        }

        private async Task<List<GetPerformanceReviewForEditOutput>> GetReviewsForDeliverable(long deliverableId)
        {

            var filteredReviews = _lookup_reviewRepository.GetAll()
                                        .Where(x => x.OrganizationUnitId == deliverableId);

            var reviews = from o in filteredReviews
                          join ou in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals ou.Id into ou1
                          from ou2 in ou1.DefaultIfEmpty()

                          join m in _lookup_organizationUnitRepository.GetAll() on ou2.ParentId equals m.Id into m1
                          from m2 in m1.DefaultIfEmpty()

                          select new GetPerformanceReviewForEditOutput()
                          {
                              Review = ObjectMapper.Map<CreateOrEditPerformanceReviewDto>(o),
                              DeliverableName = ou2 == null ? "" : ou2.DisplayName,
                              MdaName = m2 == null ? "" : m2.DisplayName
                          };

            return await reviews.ToListAsync();
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
