using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using PMSDemo.Authorization.Users;
using PMSDemo.MultiTenancy;

namespace PMSDemo.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}