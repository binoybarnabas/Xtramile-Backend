using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.TravelModeRepository
{
    public class TravelModeRepository : Repository<TBL_TRAVEL_MODE>, ITravelModeRepository
    {
        public TravelModeRepository(AppDBContext dbContext) : base(dbContext)
        {
        }
    }


}
