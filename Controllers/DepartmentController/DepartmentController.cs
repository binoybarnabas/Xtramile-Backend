using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.DepartmentService;

namespace XtramileBackend.Controllers.DepartmentControllers
{
    [Route("api/department")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentServices _departmentServices;

        public DepartmentController(IDepartmentServices departmentServices)
        {
            _departmentServices = departmentServices;
        }

        [HttpGet("departments")]
        public async Task<IActionResult> GetDepartmentsAsync()
        {
            try
            {
                IEnumerable<TBL_DEPARTMENT> departmentData = await _departmentServices.GetDepartmentAsync();
                return Ok(departmentData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting departments: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddDepartmentAsync([FromBody] TBL_DEPARTMENT department)
        {
            try
            {
                await _departmentServices.SetDepartmentAsync(department);
                return Ok(department);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding a department: {ex.Message}");
            }
        }
    }
}
