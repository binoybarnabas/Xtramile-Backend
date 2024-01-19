using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.EmployeeService;
using XtramileBackend.Services.TravelModeService;

namespace XtramileBackend.Controllers.TravelModeController
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelModeController : ControllerBase
    {
        private readonly ITravelModeService _travelModeService;


        public TravelModeController(ITravelModeService travelModeService)
        {
            _travelModeService = travelModeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTravelModesAsync()
        {
            try
            {
                IEnumerable<TBL_TRAVEL_MODE> travelData = await _travelModeService.GetTravelModeAsync();
                return Ok(travelData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting travel mode: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTravelModeAsync([FromBody] TBL_TRAVEL_MODE travelMode)
        {
            try
            {
                await _travelModeService.SetTravelModeAsync(travelMode);
                return Ok(travelMode);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding a travel modes: {ex.Message}");
            }


        }
    }
}
