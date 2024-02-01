using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Services.ManagerService;

namespace XtramileBackend.Controllers.ReportingManagerController
{
    [EnableCors("AllowAngularDev")]
    [Route("api/reportingmanager")]
    [ApiController]
    public class ReportingManagerController : ControllerBase
    {
        public readonly IReportingManagerService _reportingManagerService;
        public ReportingManagerController(IReportingManagerService reportingManagerService)
        {
            _reportingManagerService = reportingManagerService;
        }
        [HttpGet("request")]
        public async Task<IActionResult> GetEmployeeRequestAsync([FromQuery] int managerId)
        {
            var empRequests = await _reportingManagerService.GetEmployeeRequestsAsync(managerId);
            return Ok(empRequests);
        }

        [HttpGet("date")]
        public async Task<IActionResult> GetEmployeeRequestByDateAsync([FromQuery] int managerId, DateTime date)
        {
            var empRequests = await _reportingManagerService.GetEmployeeRequestsByDateAsync(managerId, date);
            return Ok(empRequests);
        }

        [HttpGet("email")]
        public async Task<IActionResult> GetEmployeeRequestByEmailAsync([FromQuery] int managerId, string email)
        {
            var empRequests = await _reportingManagerService.GetEmployeeRequestsByEmailAsync(managerId, email);
            return Ok(empRequests);
        }


        [HttpGet("Sort/requestcode")]
        public async Task<IActionResult> GetEmployeeRequestSortByRequestCodeAsync([FromQuery] int managerId)
        {
            var empRequests = await _reportingManagerService.GetEmployeeRequestsSortByRequestCodeAsync(managerId);
            return Ok(empRequests);

        }
        [HttpGet("sort/date")]
        public async Task<IActionResult> GetEmployeeRequestSortByDateAsync([FromQuery] int managerId)
        {
            var empRequests = await _reportingManagerService.GetEmployeeRequestsSortByDateAsync(managerId);
            return Ok(empRequests);

        }

        [HttpGet("sort/email")]
        public async Task<IActionResult> GetEmployeeRequestSortEmailAsync([FromQuery] int managerId)
        {
            var empRequests = await _reportingManagerService.GetEmployeeRequestsSortByEmailAsync(managerId);
            return Ok(empRequests);

        }
    }
}
