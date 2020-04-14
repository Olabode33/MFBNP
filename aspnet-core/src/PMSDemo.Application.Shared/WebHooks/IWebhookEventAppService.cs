using System.Threading.Tasks;
using Abp.Webhooks;

namespace PMSDemo.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
