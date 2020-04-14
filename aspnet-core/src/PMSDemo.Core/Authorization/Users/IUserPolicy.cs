using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace PMSDemo.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
