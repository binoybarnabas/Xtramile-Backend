using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.ProjectMappingService;

namespace XtramileBackend.Controllers
{
    [Route("api/projectlist")]
    [ApiController]
    public class ProjectMappingController : ControllerBase
    {
        private readonly IProjectMappingServices _projectMappingServices;

        public ProjectMappingController(IProjectMappingServices projectMappingServices)
        {
            _projectMappingServices = projectMappingServices;
        }

        [HttpGet("projectmap")]
        public async Task<IActionResult> GetPrioritiesAsync()
        {
            try
            {
                IEnumerable<ProjectEmployeeMap> projectMappingData = await _projectMappingServices.GetProjectMappingsAsync();
                return Ok(projectMappingData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting project mappings: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddProjectMappingAsync([FromBody] ProjectEmployeeMap projectMapping)
        {
            try
            {
                await _projectMappingServices.AddProjectMappingAsync(projectMapping);
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
