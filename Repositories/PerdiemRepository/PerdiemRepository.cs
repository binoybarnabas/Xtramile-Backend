using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.PerdiemRepository
{
    public class PerdiemRepository: Repository<PerDiem>, IPerdiemRepository
    {
        public PerdiemRepository(AppDBContext dbContext) : base(dbContext)
        {

        }
    }
}
