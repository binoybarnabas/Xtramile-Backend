using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.AvailableOptionRepository
{
    public class AvailableOptionRepository : Repository<TBL_AVAIL_OPTION>, IAvailableOptionRepository
    {
        public AvailableOptionRepository(AppDBContext context) : base(context)
        {

        }
    }
}
