using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetEmployeeRequestByDateAsync([FromQuery] int managerId, string     date)
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
    }
}
