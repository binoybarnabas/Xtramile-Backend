using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.InvoiceService
{
    public interface IInvoiceServices

    { 
        public IEnumerable<TBL_INVOICE> GetAllInvoices();
        public void AddInvoice(TBL_INVOICE invoice);

    }
}

