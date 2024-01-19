using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.CategoryService
{
    public interface ICategoryServices
    {
        public Task<IEnumerable<TBL_CATEGORY>> GetCategoriesAsync();
        public Task AddCategoryAsync(TBL_CATEGORY category);
    }
}
