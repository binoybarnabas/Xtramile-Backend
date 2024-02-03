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

        // Get employee requests sorted by date based on managerId
        [HttpGet("sort/date")]
        public async Task<IActionResult> GetEmployeeRequestSortByDateAsync([FromQuery] int managerId)
        {
            var empRequests = await _reportingManagerService.GetEmployeeRequestsSortByDateAsync(managerId);
            return Ok(empRequests);
        }

        // Get employee requests sorted by email based on managerId
        [HttpGet("sort/employeename")]
        public async Task<IActionResult> GetEmployeeRequestSortEmailAsync([FromQuery] int managerId)
        {
            var empRequests = await _reportingManagerService.GetEmployeeRequestsSortByEmployeeNameAsync(managerId);
            return Ok(empRequests);
        }

        // Get employee requests sorted by email based on managerId
        [HttpGet("search/employeename")]
        public async Task<IActionResult> GetEmployeeRequestByEmployeeNameAsync([FromQuery] int managerId,string employeeName)
        {
            var empRequests = await _reportingManagerService.GetEmployeeRequestsByEmployeeNameAsync(managerId,employeeName);
            return Ok(empRequests);
        }
    }
}
