using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.RequestService;

namespace XtramileBackend.Controllers.RequestControllers
{
    [EnableCors("AllowAngularDev")]
    [Route("api/request")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestServices _requestServices;

        public RequestController(IRequestServices requestServices)
        {
            _requestServices = requestServices;
        }

        [HttpGet("requests")]
        public async Task<IActionResult> GetAllRequestAsync()
        {
            try
            {
                IEnumerable<TBL_REQUEST> request = await _requestServices.GetAllRequestAsync();
                return Ok(request);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting request: {ex.Message}");
            }
        }


        //New Travel Requests
        [HttpPost("add")]
        public async Task<IActionResult> AddRequestAsync([FromBody] TBL_REQUEST request)
        {

            Console.WriteLine("==============================================================================================");
            Console.WriteLine(request);
            Console.WriteLine("==============================================================================================");

            await _requestServices.AddRequestAsync(request);
            return Ok(request);
            /*try
            {
                await _requestServices.AddRequestAsync(request);
                return Ok(request);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding a request" +
                    $": {ex.Message}");
            }*/
        }
    }
}
