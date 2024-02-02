
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.EmployeeService;
using XtramileBackend.Services.ProjectService;
using Microsoft.AspNetCore.Cors;


namespace XtramileBackend.Controllers.EmployeeController
{
    [EnableCors("AllowAngularDev")]
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeServices _employeeService;

        public EmployeeController(IEmployeeServices employeeService)

        {
            _employeeService = employeeService;
        }

        [HttpGet("employees")]
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
            try
            {
                EmployeeInfo EmployeeData = await _employeeService.GetEmployeeInfo(id);
                return Ok(EmployeeData);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting an employee: {ex.Message}");
            }

        }

        [HttpPost("add")]
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

        [HttpGet("viewpendingrequest/{empId}")]
        public async Task<IActionResult> GetPendingRequestsByEmpId(int empId)
        {
            try
            {
                IEnumerable<PendingRequetsViewEmployee> pendingRequestData = await _employeeService.GetPendingRequestsByEmpId(empId);
                return Ok(pendingRequestData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting pending requests: {ex.Message}");
            }
        }


        /// <summary>
        /// Gets the profile details of an employee by their ID.
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <returns>An IActionResult containing the employee's profile details or an error message.</returns>
        [HttpGet("/api/employee/profile/details/{employeeId}")]
        public async Task<IActionResult> GetEmployeeProfileByIdAsync(int employeeId)
        {
            try
            {
                // Call the EmployeeService to get the employee's profile details
                EmployeeProfile employeeData = await _employeeService.GetEmployeeProfileByIdAsync(employeeId);

                // Return a 200 OK response with the employee's profile data
                return Ok(employeeData);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting employee profile details: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the profile details of an employee.
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <param name="profileEdit">A ProfileEdit object containing the updated details.</param>
        /// <returns>An IActionResult indicating success or failure of the update operation.</returns>
        [HttpPatch("/api/employee/edit/profile/details/{employeeId}")]
        public async Task<IActionResult> UpdateEmployeeDetailsAsync(int employeeId, [FromBody] ProfileEdit profileEdit)
        {
            try
            {
                // Call the EmployeeService to update the employee's profile details
                await _employeeService.UpdateEmployeeDetailsAsync(employeeId, profileEdit);

                // Return a 200 OK response indicating a successful update
                return Ok(profileEdit);
            }
            catch (Exception ex)
            {
                // Handle or log the exception, then return a 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while updating employee details: {ex.Message}");
            }
        }
        [HttpGet("viewoptions/request/{reqId}")]
        public async Task<IActionResult> GetOptionsByReqId(int reqId)
        {
            try
            {
                IEnumerable<OptionCard> optionsForRequestData = await _employeeService.GetOptionsByReqId(reqId);
                return Ok(optionsForRequestData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting options for request: {ex.Message}");
            }
        }


        [HttpGet("request/history")]
        public async Task<IActionResult> GetRequestHistory(int empId)
        {
            try
            {
                IEnumerable<EmployeeViewReq>requestData = await _employeeService.GeRequestHistoryByEmpId(empId);
                return Ok(requestData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting pending requests: {ex.Message}");
            }

        }


    }
}
