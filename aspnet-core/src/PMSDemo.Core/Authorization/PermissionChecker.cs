using Abp.Authorization;
using PMSDemo.Authorization.Roles;
using PMSDemo.Authorization.Users;

namespace PMSDemo.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
