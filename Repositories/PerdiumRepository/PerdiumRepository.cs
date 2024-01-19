using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.PerdiumRepository
{
    public class PerdiumRepository: Repository<TBL_PER_DIUM>, IPerdiumRepository
    {
        public PerdiumRepository(AppDBContext dbContext) : base(dbContext)
        {

        }
    }
}
