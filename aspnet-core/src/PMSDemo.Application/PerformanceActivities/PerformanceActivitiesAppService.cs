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
using PMSDemo.PerformanceActivities.Dtos;
using PMSDemo.PriorityAreas;
using PMSDemo.Enums;

namespace PMSDemo.PerformanceActivities
{
    [AbpAuthorize(AppPermissions.Pages_PerformanceActivity)]
    public class PerformanceActivitiesAppService : PMSDemoAppServiceBase
    {
        private readonly IRepository<PerformanceActivity> _performanceActivityRepository;
        private readonly IRepository<ActivityAttachment> _activityAttachmentRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<OrganizationUnit, long> _lookup_organizationUnitRepository; 
        private readonly IRepository<PriorityArea> _lookup_priorityAreaRepository; 
        private readonly OrganizationUnitManager _organizationUnitManager;

        public PerformanceActivitiesAppService(
            IRepository<PerformanceActivity> performanceActivityRepository,
            IRepository<ActivityAttachment> activityAttachmentRepository,
            IRepository<User, long> lookup_userRepository, 
            IRepository<OrganizationUnit, long> lookup_organizationUnitRepository,
            IRepository<PriorityArea> lookup_priorityAreaRepository,
            OrganizationUnitManager organizationUnitManager)
        {
            _performanceActivityRepository = performanceActivityRepository;
            _activityAttachmentRepository = activityAttachmentRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_organizationUnitRepository = lookup_organizationUnitRepository;
            _lookup_priorityAreaRepository = lookup_priorityAreaRepository;
            _organizationUnitManager = organizationUnitManager;
        }

        public async Task<PagedResultDto<GetPerformanceActivityForEditOutput>> GetAllForUnit(GetAllPerformanceActivityInput input)
        {

            var filteredIndicator = _performanceActivityRepository.GetAll()
                                        .WhereIf(!string.IsNullOrWhiteSpace(input.FilterText), e => false || e.Name.Contains(input.FilterText) || e.Description.Contains(input.FilterText))
                                        .Where(x => x.OrganizationUnitId == input.OrganizationUnitId);

            var pagedAndFilteredIndicators = filteredIndicator
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var activities = from o in pagedAndFilteredIndicators
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

            var totalCount = await filteredIndicator.CountAsync();

            return new PagedResultDto<GetPerformanceActivityForEditOutput>(
                totalCount,
                await activities.ToListAsync()
            );
        }


        [AbpAuthorize(AppPermissions.Pages_PerformanceActivity_Edit)]
        public async Task<GetPerformanceActivityForEditOutput> GetPerformanceActivityForEdit(EntityDto input)
        {
            var activity = await _performanceActivityRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPerformanceActivityForEditOutput { PerformanceActivity = ObjectMapper.Map<CreateOrEditPerformanceActivityDto>(activity) };

            var deliverable = await _lookup_organizationUnitRepository.FirstOrDefaultAsync(activity.OrganizationUnitId);
            output.DeliverableName = deliverable.DisplayName;

            if (deliverable.ParentId != null)
            {
                var mda = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((int)deliverable.ParentId);
                output.MdaName = mda.DisplayName;
            }
            output.Attachments = await GetActivityAttachments(activity.Id);

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPerformanceActivityDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PerformanceActivity_Create)]
        protected virtual async Task Create(CreateOrEditPerformanceActivityDto input)
        {
            var activity = ObjectMapper.Map<PerformanceActivity>(input);
            await _performanceActivityRepository.InsertAsync(activity);
        }

        [AbpAuthorize(AppPermissions.Pages_PerformanceActivity_Edit)]
        protected virtual async Task Update(CreateOrEditPerformanceActivityDto input)
        {
            var activity = await _performanceActivityRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, activity);
        }

        [AbpAuthorize(AppPermissions.Pages_PerformanceActivity_Update)]
        public async Task UpdateProgress(UpdateActivityProgressDto input)
        {
            var activity = await _performanceActivityRepository.FirstOrDefaultAsync((int)input.Activity.Id);

            if (input.Activity.CompletionLevel <= 0)
            {
                activity.CompletionStatus = CompletionStatusEnum.NotStarted;
            }
            if (input.Activity.CompletionLevel > 0)
            {
                if (activity.CompletionLevel == null || activity.CompletionLevel == 0)
                {
                    activity.ActualStartDate = DateTime.Now;
                }
                activity.CompletionStatus = CompletionStatusEnum.InProgress;
            }
            if (input.Activity.CompletionLevel >= 100)
            {
                activity.CompletionStatus = CompletionStatusEnum.Completed;
                activity.ActualCompletionDate = DateTime.Now;
            }

            activity.CompletionLevel = input.Activity.CompletionLevel;
            activity.Note = input.Activity.Note;
            activity.DataSource = input.Activity.DataSource;
            await _performanceActivityRepository.UpdateAsync(activity);
            await SaveAttachment(activity.Id, input.Attachments);
        }

        [AbpAuthorize(AppPermissions.Pages_PerformanceActivity_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _performanceActivityRepository.DeleteAsync(input.Id);
        }

        protected async Task SaveAttachment(int activityId, List<ActivityAttachmentDto> input)
        {
            await _activityAttachmentRepository.HardDeleteAsync(x => x.PerformanceActivityId == activityId);
            foreach (var item in input)
            {
                var attachment = ObjectMapper.Map<ActivityAttachment>(item);
                await _activityAttachmentRepository.InsertAsync(attachment);
            }
        }

        protected async Task<List<ActivityAttachmentDto>> GetActivityAttachments(int activityId)
        {
            var attachments = await _activityAttachmentRepository.GetAll()
                                        .Where(x => x.PerformanceActivityId == activityId)
                                        .Select(x => ObjectMapper.Map<ActivityAttachmentDto>(x))
                                        .ToListAsync();

            return attachments;
        }

    }
}
