using System.Collections.Generic;
using PMSDemo.Authorization.Permissions.Dto;

namespace PMSDemo.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}