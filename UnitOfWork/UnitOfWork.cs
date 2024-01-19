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
using XtramileBackend.Repositories.FileTypeRepository;
using XtramileBackend.Repositories.ReasonRepository;
using XtramileBackend.Repositories.RequestRepository;
using XtramileBackend.Repositories.StatusRepository;
using XtramileBackend.Repositories.TravelModeRepository;
using XtramileBackend.Repositories.TravelTypeRepository;

namespace XtramileBackend.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
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
        public IFileTypeRepository FileTypeRepository { get; }
        public IReasonRepository ReasonRepository { get; }
        public IStatusRepository StatusRepository { get; }
        public IRequestRepository RequestRepository { get; }

        public ITravelTypeRepository TravelTypeRepository { get; }

        public ITravelModeRepository TravelModeRepository { get; }

        public readonly AppDBContext _dbContext;
        public UnitOfWork(AppDBContext dbContext)
        {
            _dbContext = dbContext;
            PriorityRepository = new PriorityRepository(_dbContext);
            ProjectRepository = new ProjectRepository(_dbContext);
            DepartmentRepository = new DepartmentRepository(_dbContext);
            InvoiceRepository = new InvoiceRepository(_dbContext);
            ExpenseRepository = new ExpenseRepository(_dbContext);
            EmployeeRepository = new EmployeeRepository(_dbContext);
            RoleRepository = new RoleRepository(_dbContext);
            CountryRepository = new CountryRepository(_dbContext);
            PerdiumRepository = new PerdiumRepository(_dbContext);
            FileTypeRepository = new FileTypeRepository(_dbContext);
            ReasonRepository = new ReasonRepository(_dbContext);
            StatusRepository = new StatusRepository(_dbContext);
            RequestRepository = new RequestRepository(_dbContext);
        }
            TravelModeRepository = new TravelModeRepository(_dbContext);
            TravelTypeRepository = new TravelTypeRepository(_dbContext);    
        } 

        public int Complete()
        {
            return _dbContext.SaveChanges();
        }

    }
}
