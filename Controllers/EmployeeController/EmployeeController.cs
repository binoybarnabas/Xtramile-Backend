using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.EmployeeService;
using XtramileBackend.Services.ProjectService;

namespace XtramileBackend.Controllers.EmployeeController
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeServices _employeeService;
        public EmployeeController(IEmployeeServices employeeService, IProjectServices projectService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeesAsync()
        {
            try
            {
                IEnumerable<TBL_EMPLOYEE> employeeData = await _employeeService.GetEmployeeAsync();
               
                return Ok(employeeData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting employees: {ex.Message}");
            }
        }

        [HttpGet("info/{id}")]
        public async Task<IActionResult> GetEmployeeInfo(int id)
        {
            try {   
                EmployeeInfo EmployeeData = await _employeeService.GetEmployeeInfo(id);
                return Ok(EmployeeData);
            
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding a employee: {ex.Message}");
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployeeAsync([FromBody] TBL_EMPLOYEE employee)
        {
            try
            {
                await _employeeService.SetEmployeeAsync(employee);
                return Ok(employee);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding a employee: {ex.Message}");
            }
        }
    }
}
