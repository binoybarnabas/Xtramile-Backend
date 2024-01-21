using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.RequestStatusRepository
{
    public class RequestStatusRepository : Repository<TBL_REQ_APPROVE>, IRequestStatusRepository
    {

        public RequestStatusRepository(AppDBContext dbContext) : base(dbContext)
        {

        }

    }
}