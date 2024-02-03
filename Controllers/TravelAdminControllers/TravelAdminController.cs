using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.APIModels;
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

        [HttpGet("incomingrequests")]
        public async Task<IActionResult> GetIncomingRequests()
        {
            try
            {
                var incomingRequestData = await _travelAdminService.GetIncomingRequests();
                return Ok(incomingRequestData);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting incoming requests: {ex.Message}");
            }
        }

        [HttpGet("options/selected")]
        public async Task<IActionResult> GetSelectedOption(int reqId)
        {
            try
            {
                OptionCard option = await _travelAdminService.GetSelectedOptionFromEmployee(reqId);
                return Ok(option);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting options for request: {ex.Message}");
            }
        }
        [HttpGet("requestsView/{statusCode}")]
        public async Task<IActionResult> viewTravelRequestByCode(string statusCode)
        {
            try
            {
                IEnumerable<RequestTableViewTravelAdmin> requestData = await _travelAdminService.GetTravelRequests(statusCode);
                return Ok(requestData);
            }
            catch(Exception ex)
            {
                {
                    // Handle or log the exception
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting options for request: {ex.Message}");
                }
            }

        }

    }
}
