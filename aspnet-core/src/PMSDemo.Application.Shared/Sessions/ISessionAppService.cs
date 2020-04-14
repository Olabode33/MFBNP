using System.Threading.Tasks;
using Abp.Application.Services;
using PMSDemo.Sessions.Dto;

namespace PMSDemo.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
