﻿using Abp.AspNetCore.Mvc.Authorization;
using PMSDemo.Authorization;
using PMSDemo.Storage;
using Abp.BackgroundJobs;

namespace PMSDemo.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}