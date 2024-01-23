using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Services.EmployeeViewPenReqService;

namespace XtramileBackend.Controllers
{
    [EnableCors("AllowAngularDev")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmpViewController : ControllerBase
    {
        private readonly IEmployeeViewPenReqService? _employeeViewPenReqService;
        public EmpViewController(IEmployeeViewPenReqService employeeViewPenReqService)
        {
            _employeeViewPenReqService = employeeViewPenReqService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPendingRequestsByEmpId(int empId)
        {
            try
            {
                IEnumerable<object> pendingRequestData = await _employeeViewPenReqService.GetPendingRequestsByEmpId(empId);
                return Ok(pendingRequestData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting pending requests: {ex.Message}");
            }
        }

    }
}
