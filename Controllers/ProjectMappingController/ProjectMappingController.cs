using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.EmployeeService;
using XtramileBackend.Services.ProjectMappingService;

namespace XtramileBackend.Controllers.ProjectMappingController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectMappingController : ControllerBase
    {
        private readonly IProjectMappingService _projectMappingService;

        public ProjectMappingController(IProjectMappingService projectMappingService)
        {
            _projectMappingService = projectMappingService; 
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeesAsync()
        {
            try
            {
                IEnumerable<TBL_PROJECT_MAPPING> projectMapping = await _projectMappingService.GetProjectMappingAsync();
                return Ok(projectMapping);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting project mapping: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployeeAsync([FromBody] TBL_PROJECT_MAPPING projectMapping)
        {
            try
            {
                await _projectMappingService.SetProjectMappingAsync(projectMapping);
                return Ok(projectMapping);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding a project mapping: {ex.Message}");
            }
        }

    }
}
