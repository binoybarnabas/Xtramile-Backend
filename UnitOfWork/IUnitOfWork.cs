using XtramileBackend.Data;
using XtramileBackend.Repositories.CountryRepository;
using XtramileBackend.Repositories.DepartmentRepository;
using XtramileBackend.Repositories.EmployeeRepository;
using XtramileBackend.Repositories.ExpenseRepository;
using XtramileBackend.Repositories.InvoiceRepository;
using XtramileBackend.Repositories.PerdiumRepository;
using XtramileBackend.Repositories.PriorityRepository;
using XtramileBackend.Repositories.ProjectRepository;
using XtramileBackend.Repositories.RoleRepository;

namespace XtramileBackend.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IPriorityRepository PriorityRepository { get; }
        public IProjectRepository ProjectRepository { get; }
        public IDepartmentRepository DepartmentRepository { get; }
        public IInvoiceRepository InvoiceRepository { get; }
        public IExpenseRepository ExpenseRepository { get; }
        public IEmployeeRepository EmployeeRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public ICountryRepository CountryRepository { get; }
        public IPerdiumRepository PerdiumRepository { get; }
        public int Complete();
        //public void Dispose();
    }
}
