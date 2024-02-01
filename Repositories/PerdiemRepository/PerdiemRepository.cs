using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.PerdiemRepository
{
    public class PerdiemRepository: Repository<TBL_PER_DIEM>, IPerdiemRepository
    {
        public PerdiemRepository(AppDBContext dbContext) : base(dbContext)
        {

        }
    }
}
