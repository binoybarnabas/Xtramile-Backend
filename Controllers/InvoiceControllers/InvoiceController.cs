using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.InvoiceService;

namespace XtramileBackend.Controllers.ProductControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceServices _invoiceServices;

        public InvoiceController(IInvoiceServices invoiceServices)
        { 
            _invoiceServices = invoiceServices;
        }

        [HttpGet]
        public IActionResult GetInvoices() {
            var ProductsData = _invoiceServices.GetAllInvoices();
            return Ok(ProductsData);
        }

        [HttpPost]
        public IActionResult AddInvoice([FromBody] TBL_INVOICE invoice)
        {
            _invoiceServices.AddInvoice(invoice);
            return Ok(invoice);
        }
    }
}
