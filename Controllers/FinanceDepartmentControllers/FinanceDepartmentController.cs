using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Services.FinanceDepartment;

namespace XtramileBackend.Controllers.FinanceDepartmentControllers
{
    [Route("api/financedepartment")]
    [ApiController]
    public class FinanceDepartmentController : ControllerBase
    {
        private readonly IFinanceDepartmentService _financeDepartmentService;
        public FinanceDepartmentController(IFinanceDepartmentService financeDepartmentService)
        {
            _financeDepartmentService = financeDepartmentService;
        }

        [HttpGet("getrequests")]
        public async Task<IActionResult> GetRequests()
        {
            try
            {
                var requestsData = await _financeDepartmentService.GetIncomingRequests();
                return Ok(requestsData);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting requests: {ex.Message}");
            }

        }

        [HttpGet("getrequests/sort")]
        public async Task<IActionResult> GetRequests([FromQuery] string sortField, bool isDescending)
        {
            try
            {
                var requestsData = await _financeDepartmentService.SortIncomingList(sortField, isDescending);
                return Ok(requestsData);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting requests: {ex.Message}");
            }

        }

        [HttpGet("invoices")]
        public async Task<IActionResult> GetInvoiceAttachements()
        {
            try
            {
                var invoiceData = await _financeDepartmentService.GetAllInvoiceAttachments();
                return Ok(invoiceData);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting Invoices: {ex.Message}");
            }

        }

        [HttpPatch("updatestatus/{id}")]
        public async Task<IActionResult> UpdateInvoiceStatus(int id, [FromBody] InvoiceStatus invoiceStatus)
        {
            try
            {
                var statusUpdate = await _financeDepartmentService.UpdateInvoiceStatus(id, invoiceStatus);

                return Ok("Updated the list");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting Invoices: {ex.Message}");
            }

        }


        [HttpGet("invoice/updatedstatus")]
        public async Task<IActionResult> GetInvoiceOnStatus([FromQuery] bool isUtr)
        {
            try
            {
                var statusList = await _financeDepartmentService.GetInvoicesBasedOnStatus(isUtr);
                return Ok(statusList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting Invoices: {ex.Message}");
            }

        }
    }
}
