using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Services.ManagerService;

// Controller for handling reporting manager related actions
namespace XtramileBackend.Controllers.ReportingManagerController
{
    [EnableCors("AllowAngularDev")]
    [Route("api/reportingmanager")]
    [ApiController]
    public class ReportingManagerController : ControllerBase
    {
        public readonly IReportingManagerService _reportingManagerService;

        // Constructor to inject the Reporting Manager Service
        public ReportingManagerController(IReportingManagerService reportingManagerService)
        {
            _reportingManagerService = reportingManagerService;
        }

        // Get employee requests based on managerId
        [HttpGet("request")]
        public async Task<IActionResult> GetEmployeeRequestAsync([FromQuery] int managerId)
        {
            var empRequests = await _reportingManagerService.GetEmployeeRequestsAsync(managerId);
            return Ok(empRequests);
        }

        // Get employee requests for a specific date based on managerId and date
        [HttpGet("date")]
        public async Task<IActionResult> GetEmployeeRequestByDateAsync([FromQuery] int managerId, string date)
        {
            var empRequests = await _reportingManagerService.GetEmployeeRequestsByDateAsync(managerId, date);
            return Ok(empRequests);
        }

        // Get employee requests for a specific email based on managerId and email
        [HttpGet("email")]
        public async Task<IActionResult> GetEmployeeRequestByEmailAsync([FromQuery] int managerId, string email)
        {
            var empRequests = await _reportingManagerService.GetEmployeeRequestsByEmailAsync(managerId, email);
            return Ok(empRequests);
        }

        // Get employee requests sorted by request code based on managerId
        [HttpGet("sort/requestcode")]
        public async Task<IActionResult> GetEmployeeRequestSortByRequestCodeAsync([FromQuery] int managerId)
        {
            var empRequests = await _reportingManagerService.GetEmployeeRequestsSortByRequestCodeAsync(managerId);
            return Ok(empRequests);
        }

        // Get employee requests sorted by date based on managerId
        [HttpGet("sort/date")]
        public async Task<IActionResult> GetEmployeeRequestSortByDateAsync([FromQuery] int managerId)
        {
            var empRequests = await _reportingManagerService.GetEmployeeRequestsSortByDateAsync(managerId);
            return Ok(empRequests);
        }

        // Get employee requests sorted by email based on managerId
        [HttpGet("sort/email")]
        public async Task<IActionResult> GetEmployeeRequestSortEmailAsync([FromQuery] int managerId)
        {
            var empRequests = await _reportingManagerService.GetEmployeeRequestsSortByEmailAsync(managerId);
            return Ok(empRequests);
        }

        /// <summary>
        /// Retrieves ongoing travel request details for employees reporting to a specific manager.
        /// </summary>
        /// <param name="managerId">The ID of the reporting manager.</param>
        /// <returns>
        /// 200 OK response with a collection of ongoing travel request details for employees reporting to the specified manager,
        /// or 500 Internal Server Error response in case of an exception.
        /// </returns>
        [HttpGet("ongoing/travel/request/{managerId}")]
        public async Task<IActionResult> GetManagerOngoingTravelRequest(int managerId)
        {
            try
            {
                // Call the service method to retrieve ongoing travel request details for employees reporting to the specified manager
                IEnumerable<ManagerOngoingTravelRequest> ongoingTravelRequestData = await _reportingManagerService.GetManagerOngoingTravelRequestDetails(managerId);

                // Return a 200 OK response with the retrieved ongoing travel request details
                return Ok(ongoingTravelRequestData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting ongoing travel request details: {ex.Message}");
            }
        }

    }
}
