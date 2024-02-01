using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.InvoiceService;

namespace XtramileBackend.Controllers.InvoiceControllers
{
    [Route("api/invoice")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceServices _invoiceServices;

        public InvoiceController(IInvoiceServices invoiceServices)
        {
            _invoiceServices = invoiceServices;
        }

        [HttpGet("invoices")]
        public async Task<IActionResult> GetPrioritiesAsync()
        {
            try
            {
                IEnumerable<TBL_INVOICE> invoiceData = await _invoiceServices.GetInvoicesAsync();
                return Ok(invoiceData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting invoices: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddInvoiceAsync([FromBody] TBL_INVOICE invoice)
        {
            try
            {
                await _invoiceServices.AddInvoiceAsync(invoice);
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding an invoice: {ex.Message}");
            }
        }
    }
}
