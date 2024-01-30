using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.TravelTypeService;

namespace XtramileBackend.Controllers.TravelTypeController
{
    [Route("api/travletype")]
    [ApiController]
    public class TravelTypeController : ControllerBase
    {
        private readonly ITravelTypeService _travelTypeService;


        public TravelTypeController(ITravelTypeService travelTypeService)
        {
            _travelTypeService = travelTypeService;
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetTravelTypeAsync()
        {
            try
            {
                IEnumerable<TBL_TRAVEL_TYPE> travelData = await _travelTypeService.GetTravelTypeAsync();
                return Ok(travelData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting travel type: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddTravelTypeAsync([FromBody] TBL_TRAVEL_TYPE travelType)
        {
            try
            {
                await _travelTypeService.SetTravelTypeAsync(travelType);
                return Ok(travelType);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding a travel types: {ex.Message}");
            }
        }
    }
}
