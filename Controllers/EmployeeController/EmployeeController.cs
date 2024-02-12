
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.EmployeeService;
using XtramileBackend.Services.ProjectService;
using Microsoft.AspNetCore.Cors;
using XtramileBackend.Services.FileTypeService;
using XtramileBackend.Services.FileMetaDataService;


namespace XtramileBackend.Controllers.EmployeeController
{
    [EnableCors("AllowAngularDev")]
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeServices _employeeService;


        private readonly IFileTypeServices _fileTypeServices;

        private readonly IFileMetaDataService _fileMetaDataServices;

        public EmployeeController(IEmployeeServices employeeService, IFileTypeServices fileTypeServices,
            IFileMetaDataService fileMetaDataServices)

        {
            _employeeService = employeeService;
            _fileTypeServices = fileTypeServices;
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
        [HttpGet("profile/details/{employeeId}")]
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
        [HttpPatch("edit/profile/details/{employeeId}")]
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
        public async Task<IActionResult> GetRequestHistory(int empId, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                PagedEmployeeViewReqDto requestData = await _employeeService.GeRequestHistoryByEmpId(empId, pageIndex, pageSize);
                return Ok(requestData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting pending requests: {ex.Message}");
            }

        }


        [HttpPost("add/option")]
        public async Task<IActionResult> AddSelectedOptionForRequest([FromBody] TBL_REQ_MAPPING option)
        {
            try
            {
                await _employeeService.AddSelectedOptionForRequest(option);
                return Ok(option);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding an option: {ex.Message}");
            }
        }

        /// <summary>
        /// Controller for handling ongoing request details for an employee.
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <returns>An IActionResult indicating success or failure of the get operation.</returns>
        [HttpGet("ongoing/request/{employeeId}")]
        public async Task<IActionResult> GetEmployeeOngoingRequest(int employeeId)
        {
            try
            {
                // Call the service method to retrieve ongoing request details for the specified employee
                IEnumerable<EmployeeOngoingRequest> employeeOngoingData = await _employeeService.GetEmployeeOngoingRequestDetails(employeeId);

                // Return a 200 OK response with the retrieved ongoing request details
                return Ok(employeeOngoingData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting ongoing request details: {ex.Message}");
            }
        }

        [HttpGet("current/request")]
        public async Task<IActionResult> getEmployeeCurrentTravel(int empId)
        {
            try
            {
                EmployeeCurrentRequest request = await _employeeService.getEmployeeCurrentTravel(empId);
                return Ok(request);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error while getting currently travelling request: {ex.Message}");
            }
        }

        [HttpPatch("update/password")]
        public async Task<IActionResult> updatePassword([FromBody] UpdatePassword updatePassword)
        {
            try
            {
                var user = await _employeeService.updatePassword(updatePassword.Email, updatePassword.Password);

                if (user != null)
                {
                    // Password updated successfully
                    return Ok("Password updated successfully.");
                }
                else
                {
                    // User not found with the given email
                    return NotFound("User not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured while updating password: {ex.Message}");
            }

        }
        [HttpGet("dashboard/upcoming/trip/{employeeId}")]
        public async Task<IActionResult> GetEmployeeDashboardUpcomingTrip(int employeeId)
        {
            try
            {
                // Call the service method to retrieve upcoming trip details for the specified employee
                IEnumerable<DashboardUpcomingTrip> employeeDashboardUpcomingTripData = await _employeeService.GetEmployeeDashboardUpcomingTripByIdAsync(employeeId);

                // Return a 200 OK response with the retrieved upcoming trip details
                return Ok(employeeDashboardUpcomingTripData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting upcoming trip details: {ex.Message}");
            }
        }
        [HttpGet("dashboard/employee/progress/{employeeId}")]
        public async Task<IActionResult> GetEmployeeDashboardProgress(int employeeId)
        {
            try
            {
                // Call the service method to retrieve progress details for the specified employee
                DashboardEmployeeprogress employeeDashboardProgress = await _employeeService.GetEmployeeDashboardProgressAsync(employeeId);

                // Return a 200 OK response with the retrieved progress details
                return Ok(employeeDashboardProgress);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting progress details: {ex.Message}");
            }
        }

    }
}

