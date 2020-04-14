using System.Threading.Tasks;
using Abp.Application.Services;
using PMSDemo.Editions.Dto;
using PMSDemo.MultiTenancy.Dto;

namespace PMSDemo.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}