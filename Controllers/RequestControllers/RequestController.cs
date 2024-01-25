﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.RequestService;

namespace XtramileBackend.Controllers.RequestControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestServices _requestServices;

        public RequestController(IRequestServices requestServices)
        {
            _requestServices = requestServices;
        }

        [HttpGet]
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

        [HttpPost]
        public async Task<IActionResult> AddRequestAsync([FromBody] TBL_REQUEST request)
        {
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
