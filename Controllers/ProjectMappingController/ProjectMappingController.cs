using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.ProjectMappingService;

namespace XtramileBackend.Controllers.ProjectMappingControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectMappingController : ControllerBase
    {
        private readonly IProjectMappingServices _projectMappingServices;

        public ProjectMappingController(IProjectMappingServices projectMappingServices)
        {
            _projectMappingServices = projectMappingServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetPrioritiesAsync()
        {
            try
            {
                IEnumerable<TBL_PROJECT_MAPPING> projectMappingData = await _projectMappingServices.GetProjectMappingsAsync();
                return Ok(projectMappingData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting project mappings: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProjectMappingAsync([FromBody] TBL_PROJECT_MAPPING projectMapping)
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
