using XtramileBackend.Data;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Repositories.CategoryRepository
{
    public class CategoryRepository : Repository<TBL_CATEGORY>, ICategoryRepository
    {

        public CategoryRepository(AppDBContext dbContext) : base(dbContext)
        {

        }

    }
}
