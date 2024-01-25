using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Services.FinanceDepartment;

namespace XtramileBackend.Controllers.FinanceDepartmentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceDepartmentController : ControllerBase
    {
        private readonly IFinanceDepartmentService _financeDepartmentService;
        public FinanceDepartmentController(IFinanceDepartmentService financeDepartmentService)
        {
            _financeDepartmentService = financeDepartmentService;
        }

        [HttpGet("/getrequests")]
        public async Task<IActionResult> GetRequests()
        {
            try
            {
                var requestsData = await _financeDepartmentService.GetIncomingRequests();
                return Ok(requestsData);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting employees: {ex.Message}");
            }

        }

        [HttpGet("/getrequests/sort")]
        public async Task<IActionResult> GetRequests([FromQuery] string sortField, bool isDescending)
        {
            try
            {
                var requestsData = await _financeDepartmentService.SortIncomingList(sortField, isDescending);
                return Ok(requestsData);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting employees: {ex.Message}");
            }

        }
    }
}
