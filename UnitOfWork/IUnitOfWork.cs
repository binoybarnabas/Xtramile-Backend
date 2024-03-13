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
using XtramileBackend.Repositories.ProjectMappingRepository;
using XtramileBackend.Repositories.RequestStatusRepository;
using XtramileBackend.Repositories.RequestMappingRepository;
using XtramileBackend.Repositories.FileMetaDataRepository;
using XtramileBackend.Repositories.TravelOptionRepository;
using XtramileBackend.Repositories.TravelOptionMappingRepository;
using XtramileBackend.Repositories.TravelDocumentFileData;

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
        public IPerdiemRepository PerdiemRepository { get; }
        public IFileTypeRepository FileTypeRepository { get; }
        public IReasonRepository ReasonRepository { get; }
        public IStatusRepository StatusRepository { get; }
        public IRequestRepository RequestRepository { get; }
        public ITravelModeRepository TravelModeRepository { get; }
        public ITravelTypeRepository TravelTypeRepository { get; }
        public IAvailableOptionRepository AvailableOptionRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IProjectMappingRepository ProjectMappingRepository { get; }
        public IRequestStatusRepository RequestStatusRepository { get; }
        public IRequestMappingRepsitory RequestMappingRepository { get; }
        public IFileMetaDataRepository FileMetaDataRepository { get; }
        public ITravelOptionRepository TravelOptionRepository { get; }
        public ITravelOptionMappingRepository TravelOptionMappingRepository { get; }
        public ITravelDocumentFileDataRepository TravelDocumentFileDataRepository { get; }



        public int Complete();
        Task SaveChangesAsyn();
        //public void Dispose();
    }
}
