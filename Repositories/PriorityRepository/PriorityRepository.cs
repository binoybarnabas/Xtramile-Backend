using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.PriorityRepository
{
    public class PriorityRepository : Repository<Priority>, IPriorityRepository
    {

        public PriorityRepository(AppDBContext dbContext) : base(dbContext){ 
       
        }

    }
}
