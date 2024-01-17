using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.PriorityService;

namespace XtramileBackend.Controllers.PriorityControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriorityController : ControllerBase
    {
        private readonly IPriorityServices _priorityServices;

        public PriorityController(IPriorityServices priorityServices)
        {
            _priorityServices = priorityServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetPrioritiesAsync()
        {
            try
            {
                IEnumerable<TBL_PRIORITY> priorityData = await _priorityServices.GetPrioritiesAsync();
                return Ok(priorityData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting priorities: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPriorityAsync([FromBody] TBL_PRIORITY priority)
        {
            try
            {
                await _priorityServices.AddPriorityAsync(priority);
                return Ok(priority);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding a priority: {ex.Message}");
            }
        }
    }
}
