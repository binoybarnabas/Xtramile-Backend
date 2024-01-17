using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.DepartmentRepository
{
    public class DepartmentRepository : Repository<TBL_DEPARTMENT>, IDepartmentRepository
    {
        public DepartmentRepository(AppDBContext context) : base(context) { 
        
        }
    }
}
