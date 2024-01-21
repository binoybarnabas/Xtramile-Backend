using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.CategoryService;

namespace XtramileBackend.Controllers.CategoryControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;

        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetPrioritiesAsync()
        {
            try
            {
                IEnumerable<TBL_CATEGORY> categoryData = await _categoryServices.GetCategoriesAsync();
                return Ok(categoryData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting categories: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCategoryAsync([FromBody] TBL_CATEGORY category)
        {
            try
            {
                await _categoryServices.AddCategoryAsync(category);
                return Ok(category);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding a category: {ex.Message}");
            }
        }
    }
}
