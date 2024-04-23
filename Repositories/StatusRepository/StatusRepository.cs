using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.StatusRepository
{
    public class StatusRepository : Repository<Status>, IStatusRepository
    {
        public StatusRepository(AppDBContext dbContext) : base(dbContext)
        {

        }
    }
}
