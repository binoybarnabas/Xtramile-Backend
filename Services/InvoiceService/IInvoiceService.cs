using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.InvoiceService
{
    public interface IInvoiceServices
    {

        public Task<IEnumerable<Invoice>> GetInvoicesAsync();
        public Task AddInvoiceAsync(Invoice invoice);
    }
}
