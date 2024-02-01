using XtramileBackend.Data;
using XtramileBackend.Repositories.CountryRepository;
using XtramileBackend.Repositories.DepartmentRepository;
using XtramileBackend.Repositories.EmployeeRepository;
using XtramileBackend.Repositories.ExpenseRepository;
using XtramileBackend.Repositories.InvoiceRepository;
using XtramileBackend.Repositories.PerdiemRepository;
using XtramileBackend.Repositories.PriorityRepository;
using XtramileBackend.Repositories.ProjectRepository;
using XtramileBackend.Repositories.RoleRepository;
using XtramileBackend.Repositories.FileTypeRepository;
using XtramileBackend.Repositories.ReasonRepository;
using XtramileBackend.Repositories.RequestRepository;
using XtramileBackend.Repositories.StatusRepository;
using XtramileBackend.Repositories.TravelModeRepository;
using XtramileBackend.Repositories.TravelTypeRepository;
using XtramileBackend.Repositories.AvailableOptionRepository;
using XtramileBackend.Repositories.CategoryRepository;
using XtramileBackend.Repositories.RequestStatusRepository;
using XtramileBackend.Repositories.ProjectMappingRepository;
using XtramileBackend.Repositories.RequestMappingRepository;

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
        public IPerdiemRepository PerdiemRepository { get; }
        public IFileTypeRepository FileTypeRepository { get; }
        public IReasonRepository ReasonRepository { get; }
        public IStatusRepository StatusRepository { get; }
        public IRequestRepository RequestRepository { get; }
        public ITravelTypeRepository TravelTypeRepository { get; }
        public ITravelModeRepository TravelModeRepository { get; }
        public IAvailableOptionRepository AvailableOptionRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IRequestStatusRepository RequestStatusRepository { get; }
        public IProjectMappingRepository ProjectMappingRepository { get; }
        public IRequestMappingRepsitory RequestMappingRepository { get; }


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
            PerdiemRepository = new PerdiemRepository(_dbContext);
            FileTypeRepository = new FileTypeRepository(_dbContext);
            ReasonRepository = new ReasonRepository(_dbContext);
            StatusRepository = new StatusRepository(_dbContext);
            RequestRepository = new RequestRepository(_dbContext);
            TravelModeRepository = new TravelModeRepository(_dbContext);
            TravelTypeRepository = new TravelTypeRepository(_dbContext);
            AvailableOptionRepository = new AvailableOptionRepository(_dbContext);
            CategoryRepository = new CategoryRepository(_dbContext);
            RequestStatusRepository = new RequestStatusRepository(_dbContext);
            ProjectMappingRepository = new ProjectMappingRepository(_dbContext);
            RequestMappingRepository = new RequestMappingRepository(_dbContext);
        } 

        public int Complete()
        {
            return _dbContext.SaveChanges();
        }
        public Task SaveChangesAsyn()
        {
            return _dbContext.SaveChangesAsync();
        }

    }
}
