using System.Threading.Tasks;
using Abp.Application.Services;
using PMSDemo.MultiTenancy.Payments.PayPal.Dto;

namespace PMSDemo.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
