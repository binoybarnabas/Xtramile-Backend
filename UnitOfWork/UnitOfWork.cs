using XtramileBackend.Data;
using XtramileBackend.Repositories.DepartmentRepository;
using XtramileBackend.Repositories.PriorityRepository;
using XtramileBackend.Repositories.ProjectRepository;

namespace XtramileBackend.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        public IPriorityRepository PriorityRepository { get; }
        public IProjectRepository ProjectRepository { get; }
        public IDepartmentRepository DepartmentRepository { get; }

        public readonly AppDBContext _dbContext;
        public UnitOfWork(AppDBContext dbContext) { 
            _dbContext = dbContext;
            PriorityRepository = new PriorityRepository(_dbContext);
            ProjectRepository = new ProjectRepository(_dbContext);
            DepartmentRepository = new DepartmentRepository(_dbContext);
        } 

        public int Complete() {
            return _dbContext.SaveChanges();
        }

    }
}
