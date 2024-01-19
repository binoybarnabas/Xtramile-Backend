using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.CategoryService
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<TBL_CATEGORY>> GetCategoriesAsync()
        {
            try
            {
                var categoryData = await _unitOfWork.CategoryRepository.GetAllAsync();
                return categoryData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting categories: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task AddCategoryAsync(TBL_CATEGORY category)
        {
            try
            {
                await _unitOfWork.CategoryRepository.AddAsync(category);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding a category: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}
