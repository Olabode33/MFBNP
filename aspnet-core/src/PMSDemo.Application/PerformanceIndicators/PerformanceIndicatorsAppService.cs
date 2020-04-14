﻿using System;
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
using PMSDemo.PerformanceIndicators.Dtos;
using PMSDemo.PriorityAreas;

namespace PMSDemo.PerformanceIndicators
{
    [AbpAuthorize(AppPermissions.Pages_PerformanceIndicator)]
    public class PerformanceIndicatorsAppService : PMSDemoAppServiceBase
    {
        private readonly IRepository<PerformanceIndicator> _performanceIndicatorRepository;
        private readonly IRepository<IndicatorAttachment> _indicatorAttachmentRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<OrganizationUnit, long> _lookup_organizationUnitRepository; 
        private readonly IRepository<PriorityArea> _lookup_priorityAreaRepository; 
        private readonly IRepository<IndicatorYearlyTarget> _indicatorYearlyTargetRepository; 
        private readonly OrganizationUnitManager _organizationUnitManager;

        public PerformanceIndicatorsAppService(
            IRepository<PerformanceIndicator> performanceIndicatorRepository,
            IRepository<IndicatorAttachment> indicatorAttachmentRepository,
            IRepository<User, long> lookup_userRepository, 
            IRepository<OrganizationUnit, long> lookup_organizationUnitRepository,
            IRepository<PriorityArea> lookup_priorityAreaRepository,
            IRepository<IndicatorYearlyTarget> indicatorYearlyTargetRepository,
            OrganizationUnitManager organizationUnitManager)
        {
            _performanceIndicatorRepository = performanceIndicatorRepository;
            _indicatorAttachmentRepository = indicatorAttachmentRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_organizationUnitRepository = lookup_organizationUnitRepository;
            _lookup_priorityAreaRepository = lookup_priorityAreaRepository;
            _organizationUnitManager = organizationUnitManager;
            _indicatorYearlyTargetRepository = indicatorYearlyTargetRepository;
        }

        public async Task<PagedResultDto<GetPerformanceIndicatorForEditOutput>> GetAllForUnit(GetAllPerformanceIndicatorsInput input)
        {
            //var ouCode = await _organizationUnitManager.GetCodeAsync((long)input.OrganizationUnitId);

            //string[] roots = ouCode.Split(".");
            //string previousCode = string.Empty;
            //List<string> codes = new List<string>();

            //foreach (var item in roots)
            //{
            //    previousCode = previousCode == string.Empty ? item : previousCode + "." + item;
            //    codes.Add(previousCode);
            //}

            //var units = await _lookup_organizationUnitRepository.GetAllListAsync(x => codes.Any(e => e == x.Code));

            //var filteredIndicator = _performanceIndicatorRepository.GetAll()
            //                            .WhereIf(!string.IsNullOrWhiteSpace(input.FilterText), e => false || e.Name.Contains(input.FilterText) || e.Description.Contains(input.FilterText))
            //                            .Where(x => x.OrganizationUnitId == input.OrganizationUnitId || (x.OrganizationUnitId != input.OrganizationUnitId && x.CanCascade));

            var filteredIndicator = _performanceIndicatorRepository.GetAll()
                                        .WhereIf(!string.IsNullOrWhiteSpace(input.FilterText), e => false || e.Name.Contains(input.FilterText) || e.Description.Contains(input.FilterText))
                                        .Where(x => x.OrganizationUnitId == input.OrganizationUnitId);

            var pagedAndFilteredIndicators = filteredIndicator
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var indicators = from o in pagedAndFilteredIndicators
                                join ou in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals ou.Id into ou1
                                from ou2 in ou1.DefaultIfEmpty()

                                join m in _lookup_organizationUnitRepository.GetAll() on ou2.ParentId equals m.Id into m1
                                from m2 in m1.DefaultIfEmpty()

                                select new GetPerformanceIndicatorForEditOutput()
                                {
                                    PerformanceIndicator = ObjectMapper.Map<CreateOrEditPerformanceIndicatorDto>(o),
                                    DeliverableName = ou2 == null ? "" : ou2.DisplayName,
                                    MdaName = m2 == null ? "" : m2.DisplayName,
                                    Inherited = o.OrganizationUnitId == input.OrganizationUnitId ? false : true
                                };

            //var lists = await indicators.ToListAsync();
            //lists = lists.Where(x => units.Any(e => e.Id == x.PerformanceIndicator.OrganizationUnitId))
            //             .OrderByDescending(x => x.Inherited)
            //             .ToList();

            //var totalCount = lists.Count();
            //var output = lists.Skip(input.SkipCount).Take(input.MaxResultCount);

            var totalCount = await indicators.CountAsync();

            return new PagedResultDto<GetPerformanceIndicatorForEditOutput>(
                totalCount,
                await indicators.ToListAsync()
            );
        }


        [AbpAuthorize(AppPermissions.Pages_PerformanceIndicator_Edit)]
        public async Task<GetPerformanceIndicatorForEditOutput> GetPerformanceIndicatorForEdit(EntityDto input)
        {
            var indicator = await _performanceIndicatorRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPerformanceIndicatorForEditOutput { PerformanceIndicator = ObjectMapper.Map<CreateOrEditPerformanceIndicatorDto>(indicator) };

            var deliverable = await _lookup_organizationUnitRepository.FirstOrDefaultAsync(indicator.OrganizationUnitId);
            output.DeliverableName = deliverable.DisplayName;

            if (deliverable.ParentId != null)
            {
                var mda = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((int)deliverable.ParentId);
                output.MdaName = mda.DisplayName;
            }

            output.Targets = await GetIndicatorTargets(input);


            return output;
        }

        private async Task<List<UpdateTargetDto>> GetIndicatorTargets(EntityDto input)
        {
            List<UpdateTargetDto> output = new List<UpdateTargetDto>();

            var targets = await _indicatorYearlyTargetRepository.GetAll()
                                                    .Where(x => x.IndicatorId == input.Id)
                                                    .Select(x => ObjectMapper.Map<IndicatorYearlyTargetDto>(x))
                                                    .ToListAsync();
            foreach (var target in targets)
            {
                UpdateTargetDto updateTarget = new UpdateTargetDto();
                updateTarget.Target = target;
                updateTarget.Attachments = await GetTargetAttachments(target.Id);

                output.Add(updateTarget);
            }

            return output;
        }

        public async Task CreateOrEdit(CreateEditIndicatorAndTargetDto input)
        {
            if (input.Indicator.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_PerformanceIndicator_Create)]
        protected virtual async Task Create(CreateEditIndicatorAndTargetDto input)
        {
            var indicator = ObjectMapper.Map<PerformanceIndicator>(input.Indicator);
            var id = await _performanceIndicatorRepository.InsertAndGetIdAsync(indicator);
            await SaveYearlyTargets(id, input.YearlyTarget);
        }

        [AbpAuthorize(AppPermissions.Pages_PerformanceIndicator_Edit)]
        protected virtual async Task Update(CreateEditIndicatorAndTargetDto input)
        {
            var indicator = await _performanceIndicatorRepository.FirstOrDefaultAsync((int)input.Indicator.Id);
            ObjectMapper.Map(input.Indicator, indicator);
            await SaveYearlyTargets(indicator.Id, input.YearlyTarget);
            await RemoveYearlyTarget(indicator.Id, input.YearlyTarget);
        }

        [AbpAuthorize(AppPermissions.Pages_PerformanceIndicator_Update)]
        public async Task UpdateProgress(CreateOrEditPerformanceIndicatorDto input)
        {
            var indicator = await _performanceIndicatorRepository.FirstOrDefaultAsync((int)input.Id);
            indicator.Actual = input.Actual;
            indicator.Note = input.Note;

            await _performanceIndicatorRepository.UpdateAsync(indicator);
        }

        [AbpAuthorize(AppPermissions.Pages_PerformanceIndicator_Update)]
        public async Task UpdateYearTargetProgress(UpdateTargetDto input)
        {
            var indicator = await _indicatorYearlyTargetRepository.FirstOrDefaultAsync((int)input.Target.Id);
            indicator.Actual = input.Target.Actual;
            indicator.Note = input.Target.Note;
            indicator.DataSource = input.Target.DataSource;

            await _indicatorYearlyTargetRepository.UpdateAsync(indicator);
            await SaveAttachment(indicator.Id, input.Attachments);
        }

        protected async Task SaveYearlyTargets(int indicatorId, List<IndicatorYearlyTargetDto> input)
        {
            foreach (var item in input)
            {
                var target = await _indicatorYearlyTargetRepository.FirstOrDefaultAsync(item.Id);
                if (target != null)
                {
                    ObjectMapper.Map(item, target);
                } 
                else
                {
                    var newIndicator = ObjectMapper.Map<IndicatorYearlyTarget>(item);
                    await _indicatorYearlyTargetRepository.InsertAsync(newIndicator);
                }
            }
        }

        protected async Task RemoveYearlyTarget(int indicatorId, List<IndicatorYearlyTargetDto> targets)
        {
            var indicatorTargets = await _indicatorYearlyTargetRepository.GetAll().Where(x => x.IndicatorId == indicatorId).ToListAsync();
            foreach (var item in indicatorTargets)
            {
                if (!targets.Any(x => x.Id == item.Id))
                {
                    await _indicatorYearlyTargetRepository.DeleteAsync(item.Id);
                }
            }
        }
 
        [AbpAuthorize(AppPermissions.Pages_PerformanceIndicator_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _performanceIndicatorRepository.DeleteAsync(input.Id);
        }

        protected async Task SaveAttachment(int targetId, List<IndicatorAttachmentDto> input)
        {
            await _indicatorAttachmentRepository.HardDeleteAsync(x => x.IndicatorTargetId == targetId);
            foreach (var item in input)
            {
                var attachment = ObjectMapper.Map<IndicatorAttachment>(item);
                await _indicatorAttachmentRepository.InsertAsync(attachment);
            }
        }

        protected async Task<List<IndicatorAttachmentDto>> GetTargetAttachments(int targetId)
        {
            var attachments = await _indicatorAttachmentRepository.GetAll()
                                        .Where(x => x.IndicatorTargetId == targetId)
                                        .Select(x => ObjectMapper.Map<IndicatorAttachmentDto>(x))
                                        .ToListAsync();

            return attachments;
        }

    }
}