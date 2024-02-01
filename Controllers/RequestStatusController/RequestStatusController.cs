using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.RequestStatusService;

namespace XtramileBackend.Controllers.RequestStatusControllers
{
    [Route("api/requeststatus")]
    [ApiController]
    public class RequestStatusController : ControllerBase
    {
        private readonly IRequestStatusServices _requestStatusServices;

        public RequestStatusController(IRequestStatusServices requestStatusServices)
        {
            _requestStatusServices = requestStatusServices;
        }

        [HttpGet("reqstatus")]
        public async Task<IActionResult> GetPrioritiesAsync()
        {
            try
            {
                IEnumerable<TBL_REQ_APPROVE> requestStatusData = await _requestStatusServices.GetRequestStatusesAsync();
                return Ok(requestStatusData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting request statuses: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddRequestStatusAsync([FromBody] TBL_REQ_APPROVE requestStatus)
        {
            try
            {
                await _requestStatusServices.AddRequestStatusAsync(requestStatus);
                return Ok(requestStatus);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding a request status: {ex.Message}");
            }
        }
    }
}
