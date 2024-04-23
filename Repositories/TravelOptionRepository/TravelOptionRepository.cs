using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Repositories.TravelTypeRepository;

namespace XtramileBackend.Repositories.TravelOptionRepository
{
    public class TravelOptionRepository : Repository<TravelOption>, ITravelOptionRepository
    {
        public TravelOptionRepository(AppDBContext dbContext) : base(dbContext)
        {
        }
    }
}
