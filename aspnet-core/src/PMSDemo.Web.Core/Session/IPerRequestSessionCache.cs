using System.Threading.Tasks;
using PMSDemo.Sessions.Dto;

namespace PMSDemo.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
