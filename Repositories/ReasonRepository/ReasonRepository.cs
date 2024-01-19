using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.ReasonRepository
{
    public class ReasonRepository : Repository<TBL_REASON>, IReasonRepository
    {
        public ReasonRepository(AppDBContext dbContext) : base(dbContext)
        {

        }
    }
}
