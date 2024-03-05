using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.TravelDocumentType;

namespace XtramileBackend.Controllers.TravelDocumentType
{
    [Route("api/traveldocumenttype")]
    [ApiController]
    public class TravelDocumentTypeController : ControllerBase
    {
        private readonly ITravelDocumentTypeService _travelDocumentTypeService;
        public TravelDocumentTypeController(ITravelDocumentTypeService travelDocumentTypeService) 
        { 
            _travelDocumentTypeService = travelDocumentTypeService;
        }
        [HttpGet("traveldocumenttypes")]
        public async Task<IActionResult> GetTravelDocumentTypesAsync()
        {
            try
            {
                IEnumerable<TravelDocumentTypeModel> travelDocumentTypeData = await _travelDocumentTypeService.GetTravelDocumentTypesAsync();
                return Ok(travelDocumentTypeData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting priorities: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddTravelDocumentTypeAsync([FromBody] TravelDocumentTypeModel travelDocumentType)
        {
            try
            {
                await _travelDocumentTypeService.AddTravelDocumentTypeAsync(travelDocumentType);
                return Ok(travelDocumentType);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding a priority: {ex.Message}");
            }
        }
    }
}
