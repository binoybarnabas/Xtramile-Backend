using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.AvailableOptionRepository
{
    public class AvailableOptionRepository : Repository<AvailableOption>, IAvailableOptionRepository
    {
        public AvailableOptionRepository(AppDBContext context) : base(context)
        {

        }
    }
}
