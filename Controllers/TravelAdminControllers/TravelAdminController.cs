using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Services.TravelAdminService;

namespace XtramileBackend.Controllers.TravelAdminControllers
{
    [Route("api/traveladmin")]
    [ApiController]
    public class TravelAdminController : ControllerBase
    {
        private readonly ITravelAdminService _travelAdminService;
        public TravelAdminController(ITravelAdminService travelAdminService) {
            _travelAdminService = travelAdminService;
        }

        [HttpGet("ongoing")]
        public async Task<IActionResult> GetOnGoingTravel()
        {
            try
            {
                var onGoingTravelData = await _travelAdminService.OnGoingTravel();
                return Ok(onGoingTravelData);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting on going travel updates: {ex.Message}");
            }

        }

    }
}
