using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.RequestStatusRepository
{
    public class RequestStatusRepository : Repository<RequestApprove>, IRequestStatusRepository
    {

        public RequestStatusRepository(AppDBContext dbContext) : base(dbContext)
        {

        }

    }
}