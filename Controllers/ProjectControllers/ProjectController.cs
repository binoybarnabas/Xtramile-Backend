using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.ProjectService;

namespace XtramileBackend.Controllers.ProductControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectServices _projectServices;

        public ProjectController(IProjectServices projectServices)
        { 
            _projectServices = projectServices;
        }

        [HttpGet]
        public IActionResult GetProjects() {
            var ProductsData = _projectServices.GetAllProjects();
            return Ok(ProductsData);
        }

        [HttpPost]
        public IActionResult AddProject([FromBody] TBL_PROJECT project)
        {
            _projectServices.AddProject(project);
            return Ok(project);
        }
    }
}
