using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Services.RequestMappingService;

namespace XtramileBackend.Controllers.RequestMappingController
{
    [EnableCors("AllowAngularDev")]
    [ApiController]
    [Route("api/requestmapping")]
    public class RequestMappingController : ControllerBase
    {
        private IRequestMappingService _requestMappingService;
        public RequestMappingController(IRequestMappingService requestMappingService)
        {
            _requestMappingService = requestMappingService;
        }
        [HttpGet("options/selected")]
        public async Task<IActionResult> GetSelectedOption(int reqId)
        {
            try
            {
                OptionCard option = await _requestMappingService.GetSelectedOptions(reqId);
                return Ok(option);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting options for request: {ex.Message}");
            }
        }
        [HttpPost("add/option")]
        public async Task<IActionResult> AddSelectedOptionForRequest([FromBody] TBL_REQ_MAPPING option)
        {
            try
            {
                await _requestMappingService.AddSelectedOptionForRequest(option);
                return Ok(option);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding an option: {ex.Message}");
            }
        }
    }
}
