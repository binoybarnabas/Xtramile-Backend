using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.EmployeeRepository
{
    public class EmployeeRepository : Repository<TBL_EMPLOYEE>, IEmployeeRepository
    {
           public EmployeeRepository(AppDBContext dbContext) : base(dbContext)
        {
        }
    }
}
