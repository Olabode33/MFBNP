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
using PMSDemo.Enums;
using PMSDemo.Deliverables.Dtos;

namespace PMSDemo.Deliverables
{
    [AbpAuthorize(AppPermissions.Pages_Deliverable_Review)]
    public class PerformanceReviewAppService : PMSDemoAppServiceBase
    {
        private readonly IRepository<PerformanceReview> _performanceReviewRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<OrganizationUnit, long> _lookup_organizationUnitRepository; 
        private readonly OrganizationUnitManager _organizationUnitManager;

        public PerformanceReviewAppService(
            IRepository<PerformanceReview> performanceReviewRepository, 
            IRepository<User, long> lookup_userRepository, 
            IRepository<OrganizationUnit, long> lookup_organizationUnitRepository,
            OrganizationUnitManager organizationUnitManager)
        {
            _performanceReviewRepository = performanceReviewRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_organizationUnitRepository = lookup_organizationUnitRepository;
            _organizationUnitManager = organizationUnitManager;
        }

        public async Task<PagedResultDto<GetPerformanceReviewForEditOutput>> GetAllForUnit(GetAllPerformanceReviewInput input)
        {

            var filteredReviews = _performanceReviewRepository.GetAll()
                                        .Where(x => x.OrganizationUnitId == input.OrganizationUnitId);

            var pagedAndFilteredReviews = filteredReviews
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var reviews = from o in pagedAndFilteredReviews
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

            var totalCount = await filteredReviews.CountAsync();

            return new PagedResultDto<GetPerformanceReviewForEditOutput>(
                totalCount,
                await reviews.ToListAsync()
            );
        }

        public async Task<GetPerformanceReviewForEditOutput> GetPerformanceReviewForEdit(EntityDto input)
        {
            var review = await _performanceReviewRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPerformanceReviewForEditOutput { Review = ObjectMapper.Map<CreateOrEditPerformanceReviewDto>(review) };

            var deliverable = await _lookup_organizationUnitRepository.FirstOrDefaultAsync(review.OrganizationUnitId);
            output.DeliverableName = deliverable.DisplayName;

            if (deliverable.ParentId != null)
            {
                var mda = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((int)deliverable.ParentId);
                output.MdaName = mda.DisplayName;
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPerformanceReviewDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Deliverable_Review)]
        protected virtual async Task Create(CreateOrEditPerformanceReviewDto input)
        {
            var review = ObjectMapper.Map<PerformanceReview>(input);
            await _performanceReviewRepository.InsertAsync(review);
        }

        [AbpAuthorize(AppPermissions.Pages_Deliverable_Review)]
        protected virtual async Task Update(CreateOrEditPerformanceReviewDto input)
        {
            var review = await _performanceReviewRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, review);
        }

        [AbpAuthorize(AppPermissions.Pages_PerformanceActivity_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _performanceReviewRepository.DeleteAsync(input.Id);
        }

    }
}
