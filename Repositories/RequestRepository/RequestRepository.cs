using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.RequestRepository
{
    public class RequestRepository : Repository<TBL_REQUEST>, IRequestRepository
    {
        public RequestRepository(AppDBContext dbContext) : base(dbContext)
        {

        }
    }
}
