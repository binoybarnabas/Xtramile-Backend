using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.ProjectService;
using XtramileBackend.Services.RolesService;

namespace XtramileBackend.Controllers.RolesController
{
    [Route("api/role")]
    [ApiController]
    public class RolesController : ControllerBase
    {

        private readonly IRolesServices _roleServices;

        public RolesController(IRolesServices roleServices)
        {
            _roleServices = roleServices;
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRolesAsync()
        {
            try
            {
                IEnumerable<TBL_ROLES> rolesData = await _roleServices.GetAllRolesAsync();
                return Ok(rolesData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting roles: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddRolesAsync([FromBody] TBL_ROLES roles)
        {
            try
            {
                await _roleServices.AddRoleAsync(roles);
                return Ok(roles);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding a roles: {ex.Message}");
            }
        }
    }
}
