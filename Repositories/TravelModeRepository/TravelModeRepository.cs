using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.TravelModeRepository
{
    public class TravelModeRepository : Repository<TravelMode>, ITravelModeRepository
    {
        public TravelModeRepository(AppDBContext dbContext) : base(dbContext)
        {
        }
    }


}
