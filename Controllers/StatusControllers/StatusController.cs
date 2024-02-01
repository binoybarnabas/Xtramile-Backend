using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.StatusService;

namespace XtramileBackend.Controllers.StatusControllers
{
    [Route("api/status")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusServices _statusServices;

        public StatusController(IStatusServices statusServices)
        {
            _statusServices = statusServices;
        }

        [HttpGet("statuses")]
        public async Task<IActionResult> GetAllStatusAsync()
        {
            try
            {
                IEnumerable<TBL_STATUS> status = await _statusServices.GetAllStatusAsync();
                return Ok(status);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting status: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddStatusAsync([FromBody] TBL_STATUS status)
        {
            try
            {
                await _statusServices.AddStatusAsync(status);
                return Ok(status);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding a status: {ex.Message}");
            }
        }
    }
}
