using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.ProjectService;

namespace XtramileBackend.Controllers.ProductControllers
{
    [Route("api/project")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectServices _projectServices;

        public ProjectController(IProjectServices projectServices)
        {
            _projectServices = projectServices;
        }

        [HttpGet("projects")]
        public async Task<IActionResult> GetProjectsAsync()
        {
            try
            {
                IEnumerable<TBL_PROJECT> productsData = await _projectServices.GetAllProjectsAsync();
                return Ok(productsData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting projects: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddProjectAsync([FromBody] TBL_PROJECT project)
        {
            try
            {
                await _projectServices.AddProjectAsync(project);
                return Ok(project);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding a project: {ex.Message}");
            }
        }
    }
}
