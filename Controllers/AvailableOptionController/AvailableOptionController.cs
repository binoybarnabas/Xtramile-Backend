﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.AvailableOptionService;

namespace XtramileBackend.Controllers.AvailableOptionControllers
{


    [EnableCors("AllowAngularDev")]
    [Route("api/availableoptions")]
    [ApiController]
    public class AvailableOptionController : ControllerBase
    {
        private readonly IAvailableOptionServices _availableOptionServices;

        public AvailableOptionController(IAvailableOptionServices availableOptionServices)
        {
            _availableOptionServices = availableOptionServices;
        }

        [HttpGet("traveloptions")]
        public async Task<IActionResult> GetAvailableOptionsAsync()
        {
            try
            {
                IEnumerable<TBL_AVAIL_OPTION> availableOptionData = await _availableOptionServices.GetAvailableOptionsAsync();
                return Ok(availableOptionData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting available options: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAvailableOptionAsync([FromBody] TBL_AVAIL_OPTION availableOption)
        {
            try
            {
                await _availableOptionServices.AddAvailableOptionAsync(availableOption);
                return Ok(availableOption);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding an available option: {ex.Message}");
            }
        }
    }
}
