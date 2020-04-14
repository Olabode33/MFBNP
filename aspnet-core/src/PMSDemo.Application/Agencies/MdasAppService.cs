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
using PMSDemo.Agencies.Dtos;

namespace PMSDemo.Agencies
{
    [AbpAuthorize(AppPermissions.Pages_MDA)]
    public class MdasAppService : PMSDemoAppServiceBase
    {
        private readonly IRepository<Mda, long> _mdaRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<OrganizationUnit, long> _lookup_organizationUnitRepository; 
        private readonly OrganizationUnitManager _organizationUnitManager;

        public MdasAppService(
            IRepository<Mda, long> mdaRepository, 
            IRepository<User, long> lookup_userRepository, 
            IRepository<OrganizationUnit, long> lookup_organizationUnitRepository,
            OrganizationUnitManager organizationUnitManager)
        {
            _mdaRepository = mdaRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_organizationUnitRepository = lookup_organizationUnitRepository;
            _organizationUnitManager = organizationUnitManager;
        }

        [AbpAuthorize(AppPermissions.Pages_MDA_Edit)]
        public async Task<GetMdaForEditOutput> GetMdaForEdit(EntityDto<long> input)
        {
            var mda = await _mdaRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMdaForEditOutput { Mda = ObjectMapper.Map<CreateOrEditMdaDto>(mda) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMdaDto input)
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

        [AbpAuthorize(AppPermissions.Pages_MDA_Create)]
        protected virtual async Task Create(CreateOrEditMdaDto input)
        {
            var mda = ObjectMapper.Map<Mda>(input);

            await _organizationUnitManager.CreateAsync(mda);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_MDA_Edit)]
        protected virtual async Task Update(CreateOrEditMdaDto input)
        {
            var mda = await _mdaRepository.FirstOrDefaultAsync((int)input.Id);
            mda.DisplayName = input.DisplayName;
            await _organizationUnitManager.UpdateAsync(mda);
        }

        [AbpAuthorize(AppPermissions.Pages_MDA_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _mdaRepository.DeleteAsync(input.Id);
        }


    }
}
