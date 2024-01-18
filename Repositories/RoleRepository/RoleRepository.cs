using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.RoleRepository
{
    public class RoleRepository : Repository<TBL_ROLES>, IRoleRepository
    {
        public RoleRepository(AppDBContext dbContext) : base(dbContext)
        {
        }
    }
}
