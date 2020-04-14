using Abp.Application.Services;
using Abp.Application.Services.Dto;
using PMSDemo.Authorization.Permissions.Dto;

namespace PMSDemo.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
