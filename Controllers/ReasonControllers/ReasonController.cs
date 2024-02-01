using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.ReasonService;

namespace XtramileBackend.Controllers.ReasonControllers
{
    [Route("api/reasons")]
    [ApiController]
    public class ReasonController : ControllerBase
    {
        private readonly IReasonServices _reasonServices;

        public ReasonController(IReasonServices reasonServices)
        {
            _reasonServices = reasonServices;
        }

        [HttpGet("reasons")]
        public async Task<IActionResult> GetAllReasonsAsync()
        {
            try
            {
                IEnumerable<TBL_REASON> reasons = await _reasonServices.GetAllReasonsAsync();
                return Ok(reasons);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting reason: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddReasonAsync([FromBody] TBL_REASON reasons)
        {
            try
            {
                await _reasonServices.AddReasonAsync(reasons);
                return Ok(reasons);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding a reason: {ex.Message}");
            }
        }
    }
}
