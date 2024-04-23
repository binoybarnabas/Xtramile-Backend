using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.ReasonRepository
{
    public class ReasonRepository : Repository<Reason>, IReasonRepository
    {
        public ReasonRepository(AppDBContext dbContext) : base(dbContext)
        {

        }
    }
}
