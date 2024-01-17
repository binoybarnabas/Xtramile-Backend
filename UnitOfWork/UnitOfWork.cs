using XtramileBackend.Data;
using XtramileBackend.Repositories.ExpenseRepository;
using XtramileBackend.Repositories.InvoiceRepository;

namespace XtramileBackend.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        public IExpenseRepository ExpenseRepository { get; }
        public IInvoiceRepository InvoiceRepository { get; }

        public readonly AppDBContext _dbContext;
        public UnitOfWork(AppDBContext dbContext) { 
            _dbContext = dbContext;
            ExpenseRepository = new ExpenseRepository(_dbContext);
            InvoiceRepository = new InvoiceRepository(_dbContext);
        } 

        public int Complete() {
            return _dbContext.SaveChanges();
        }

    }
}
