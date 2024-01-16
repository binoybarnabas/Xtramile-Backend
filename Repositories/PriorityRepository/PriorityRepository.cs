using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.PriorityRepository
{
    public class PriorityRepository : Repository<TBL_PRIORITY>, IPriorityRepository
    {

        public PriorityRepository(AppDBContext dbContext) : base(dbContext){ 
       
        }

    }
}
