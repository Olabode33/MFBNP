using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using PMSDemo.MultiTenancy.Accounting.Dto;

namespace PMSDemo.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
