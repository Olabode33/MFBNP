using System.Threading.Tasks;
using PMSDemo.Authorization.Users;

namespace PMSDemo.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
