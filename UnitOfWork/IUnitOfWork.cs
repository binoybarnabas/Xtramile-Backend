using XtramileBackend.Data;
using XtramileBackend.Repositories.ExpenseRepository;
using XtramileBackend.Repositories.InvoiceRepository;

namespace XtramileBackend.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IExpenseRepository ExpenseRepository { get; }
        public IInvoiceRepository InvoiceRepository { get; }

        public int Complete();
        //public void Dispose();
    }
}
