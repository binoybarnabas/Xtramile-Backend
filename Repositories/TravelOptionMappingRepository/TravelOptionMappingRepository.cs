using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Repositories.TravelModeRepository;

namespace XtramileBackend.Repositories.TravelOptionMappingRepository
{
    public class TravelOptionMappingRepository : Repository<TBL_TRAVEL_OPTION_MAPPING>, ITravelOptionMappingRepository
    {
        public TravelOptionMappingRepository(AppDBContext dbContext) : base(dbContext)
        {

        }
    }
}
