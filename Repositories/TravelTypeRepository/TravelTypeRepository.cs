using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.TravelTypeRepository
{
    public class TravelTypeRepository : Repository<TravelType>, ITravelTypeRepository
    {
        public TravelTypeRepository(AppDBContext dbContext) : base(dbContext)
        {
        }
    }
}
