using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.RequestRepository
{
    public class RequestRepository : Repository<Request>, IRequestRepository
    {
        public RequestRepository(AppDBContext dbContext) : base(dbContext)
        {

        }
    }
}
