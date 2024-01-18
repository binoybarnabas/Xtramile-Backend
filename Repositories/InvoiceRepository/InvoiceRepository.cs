using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.InvoiceRepository
{
    public class InvoiceRepository : Repository<TBL_INVOICE>, IInvoiceRepository
    {
        public InvoiceRepository(AppDBContext context) : base(context)
        {

        }
    }
}
