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
using PMSDemo.Common.Dto;
using Abp.UI;

namespace PMSDemo.PerformanceActivities
{
    [AbpAuthorize(AppPermissions.Pages_PerformanceActivity)]
    public class PerformanceActivitiesAppService : PMSDemoAppServiceBase, IPerformanceActivitiesAppService
    {
        private readonly IRepository<PerformanceActivity> _performanceActivityRepository;
        private readonly IRepository<ActivityAttachment> _activityAttachmentRepository;
        private readonly IRepository<ActivityUpdateLog> _activityProgressLogRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<OrganizationUnit, long> _lookup_organizationUnitRepository;
        private readonly IRepository<PriorityArea> _lookup_priorityAreaRepository;
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly UserManager _userManager;

        public PerformanceActivitiesAppService(
            IRepository<PerformanceActivity> performanceActivityRepository,
            IRepository<ActivityAttachment> activityAttachmentRepository,
            IRepository<ActivityUpdateLog> activityProgressLogRepository,
            IRepository<User, long> lookup_userRepository,
            IRepository<OrganizationUnit, long> lookup_organizationUnitRepository,
            IRepository<PriorityArea> lookup_priorityAreaRepository,
            UserManager userManager,
            OrganizationUnitManager organizationUnitManager)
        {
            _performanceActivityRepository = performanceActivityRepository;
            _activityAttachmentRepository = activityAttachmentRepository;
            _activityProgressLogRepository = activityProgressLogRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_organizationUnitRepository = lookup_organizationUnitRepository;
            _lookup_priorityAreaRepository = lookup_priorityAreaRepository;
            _organizationUnitManager = organizationUnitManager;
            _userManager = userManager;
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

            AuditInfoDto auditInfo = new AuditInfoDto();
            auditInfo.CreatedDate = activity.CreationTime;
            auditInfo.LastUpdatedDate = activity.LastModificationTime;
            if (activity.CreatorUserId != null)
            {
                var user = await _lookup_userRepository.FirstOrDefaultAsync((long)activity.CreatorUserId);
                auditInfo.CreatedBy = user.FullName;
            }
            if (activity.LastModifierUserId != null)
            {
                var user = await _lookup_userRepository.FirstOrDefaultAsync((long)activity.LastModifierUserId);
                auditInfo.LastUpdatedBy = user.FullName;
            }
            output.AuditInfo = auditInfo;

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
            var deliverable = await _lookup_organizationUnitRepository.FirstOrDefaultAsync(activity.OrganizationUnitId);

            var user = await _userManager.GetUserByIdAsync((long)AbpSession.UserId);
            var userOu = await _userManager.GetOrganizationUnitsAsync(user);
            var userOuCodes = userOu.Select(ou => ou.Code);

            if (!userOuCodes.Any(code => deliverable.Code.StartsWith(code)))
            {
                throw new UserFriendlyException(L("ActionOnlyAvailableToMdaMembers"));
            }

            if (input.Activity.CompletionLevel <= 0)
            {
                activity.CompletionStatus = CompletionStatusEnum.NotStarted;
            }
            if (input.Activity.CompletionLevel > 0)
            {
                if (activity.CompletionLevel == null || activity.CompletionLevel == 0)
                {
                    //activity.ActualStartDate = input.Activity.ActualStartDate;// == null ? DateTime.Now : input.Activity.ActualStartDate;
                }
                activity.CompletionStatus = CompletionStatusEnum.InProgress;
            }
            if (input.Activity.CompletionLevel >= 100)
            {
                activity.CompletionStatus = CompletionStatusEnum.Completed;
                //activity.ActualCompletionDate = input.Activity.ActualCompletionDate;// == null ? DateTime.Now : input.Activity.ActualCompletionDate;
            }

            activity.ActualStartDate = input.Activity.ActualStartDate;// == null ? DateTime.Now : input.Activity.ActualStartDate;
            activity.ActualCompletionDate = input.Activity.ActualCompletionDate;// == null ? DateTime.Now : input.Activity.ActualCompletionDate;
            activity.CompletionLevel = activity.CompletionLevel == null ? input.Activity.CompletionLevel : (activity.CompletionLevel + input.Activity.CompletionLevel) > 100 ? 100 : activity.CompletionLevel + input.Activity.CompletionLevel;
            activity.Note = input.Activity.Note;
            activity.DataSource = input.Activity.DataSource;

            await SaveProgressLog(input.Activity, activity);
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

        protected async Task SaveProgressLog(CreateOrEditPerformanceActivityDto input, PerformanceActivity activity)
        {
            await _activityProgressLogRepository.InsertAsync(new ActivityUpdateLog
            {
                PerformanceActivityId = (int)input.Id,
                CompletionLevel = input.CompletionLevel,
                OriginalValue = activity.CompletionLevel == null ? 0 : (int)activity.CompletionLevel,
                DataSource = input.DataSource,
                Notes = input.Note,
            });
        }

        public async Task<List<ActivityProgressLogDto>> GetTargetUpdateLog(EntityDto input)
        {
            var filteredLog = _activityProgressLogRepository.GetAll().Where(x => x.PerformanceActivityId == input.Id);

            var logs = from o in filteredLog
                       join u in _lookup_userRepository.GetAll() on o.CreatorUserId equals u.Id into u1
                       from u2 in u1.DefaultIfEmpty()

                       select new ActivityProgressLogDto()
                       {
                           CompletionLevel = o.CompletionLevel,
                           OriginalValue = o.OriginalValue,
                           Notes = o.Notes,
                           PerformanceActivityId = o.Id,
                           DataSource = o.DataSource,
                           LastUpdated = o.CreationTime,
                           LastUpdatedBy = u2 != null ? u2.FullName : ""
                       };

            return await logs.OrderByDescending(x => x.LastUpdated).ToListAsync();
        }

    }
}
