using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.TravelTypeRepository
{
    public class TravelTypeRepository : Repository<TBL_TRAVEL_TYPE>, ITravelTypeRepository
    {
        public TravelTypeRepository(AppDBContext dbContext) : base(dbContext)
        {
        }
    }
}
