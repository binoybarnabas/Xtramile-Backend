using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Repositories.TravelModeRepository;

namespace XtramileBackend.Repositories.TravelOptionMappingRepository
{
    public class TravelOptionMappingRepository : Repository<TravelOptionMap>, ITravelOptionMappingRepository
    {
        public TravelOptionMappingRepository(AppDBContext dbContext) : base(dbContext)
        {

        }
    }
}
