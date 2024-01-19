using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.InvoiceService
{
    public interface IInvoiceServices
    {

        public Task<IEnumerable<TBL_INVOICE>> GetInvoicesAsync();
        public Task AddInvoiceAsync(TBL_INVOICE invoice);
    }
}
