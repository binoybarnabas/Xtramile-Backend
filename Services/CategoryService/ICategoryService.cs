using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.CategoryService
{
    public interface ICategoryServices
    {
        public Task<IEnumerable<Category>> GetCategoriesAsync();
        public Task AddCategoryAsync(Category category);
    }
}
