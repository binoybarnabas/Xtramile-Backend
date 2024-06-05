using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.TravelDocumentFileData;

namespace XtramileBackend.Controllers
{
    [EnableCors("AllowAngularDev")]
    [Route("api/traveldocumentfile")]
    [ApiController]
    public class TravelDocFileDataController : ControllerBase
    {
        private readonly ITravelDocumentFileDataService _travelDocumentFileDataService;
        public TravelDocFileDataController(ITravelDocumentFileDataService travelDocumentFileDataService)
        {
            _travelDocumentFileDataService = travelDocumentFileDataService;
        }

        [HttpGet("traveldocumentfiles")]
        public async Task<IActionResult> GetTravelDocumentsOnTAScreen()
        {
            try
            {
                var httpContext = HttpContext;
                IEnumerable<TravelDocumentViewModel> travelDocumentFiles = await _travelDocumentFileDataService.GetDocumentsOnTravelAdminScreen(httpContext);
                return Ok(travelDocumentFiles);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting travel documents: {ex.Message}");
            }
        }

        [HttpGet("traveldocumentfile/{id}")]
        public async Task<IActionResult> GetTravelDocumentFilesById(int id)
        {
            try
            {
                TravelDocumentFileDataModel travelDocumentFiles = await _travelDocumentFileDataService.GetTravelDocumentFileByIdAsync(id);
                return Ok(travelDocumentFiles);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting the travel document: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddTravelDocumentFile(TravelDocument travelDocFile)
        {
            try
            {
                var httpContext = HttpContext;
                TravelDocumentFileDataModel travelDocuments = await _travelDocumentFileDataService.AddTravelDocumentFileAsync(travelDocFile, httpContext);
                return CreatedAtAction(nameof(GetTravelDocumentFilesById), new { id = travelDocuments.TravelDocFileId }, travelDocuments);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding the travel document: {ex.Message}");
            }

        }

        [HttpGet("traveldocumentfiles/{employeeId}")]
        public async Task<IActionResult> GetTravelDocumentEmployeeScreen(int employeeId)
        {
            try
            {
                var httpContext = HttpContext;
                IEnumerable<TravelDocumentViewModel> travelDocuments = await _travelDocumentFileDataService.GetDocumentDetailOnEmployeeScreen(employeeId, httpContext);
                return Ok(travelDocuments);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting the travel documents: {ex.Message}");
            }
        }

        [HttpGet("traveldocuments/{fileType}")]
        public async Task<IActionResult> GetTravelDocumentsFilteredByFileType(string fileType, int pageNumber, int itemsPerPage)
        {
            try
            {
                var httpContext = HttpContext;
                PageinatedResult<TravelDocumentViewModel> travelDocumentFiles = await _travelDocumentFileDataService.GetFilteredDocumentsOnTAScreen(fileType, httpContext, pageNumber, itemsPerPage);
                return Ok(travelDocumentFiles);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting travel documents: {ex.Message}");
            }
        }

        [HttpGet("expiredDocuments/{fileType}")]
        public async Task<IActionResult> GetExpiredDocuments(string fileType, int pageNumber, int itemsPerPage)
        {
            try
            {
                var httpContext = HttpContext;
                PageinatedResult<TravelDocumentViewModel> travelDocumentFiles = await _travelDocumentFileDataService.GetExpiredDocuments(fileType, httpContext, pageNumber, itemsPerPage);
                return Ok(travelDocumentFiles);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting travel documents: {ex.Message}");
            }
        }

        [HttpDelete("deleteDocument/{fileId}")]
        public async Task<IActionResult> DeleteDocument(int fileId)
        {
            try
            {
                await _travelDocumentFileDataService.DeleteTravelDocument(fileId);
                return Ok(new { message = "File Deleted Successfully" });
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting travel document: {ex.Message}");
            }
        }

        [HttpGet("validDocuments/{fileType}")]
        public async Task<IActionResult> GetValidDocuments(string fileType, int pageNumber, int itemsPerPage)
        {
            try
            {
                var httpContext = HttpContext;
                PageinatedResult<TravelDocumentViewModel> travelDocumentFiles = await _travelDocumentFileDataService.GetValidDocuments(fileType, httpContext, pageNumber, itemsPerPage);
                return Ok(travelDocumentFiles);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting travel documents: {ex.Message}");
            }
        }

        [HttpGet("relevantDocuments/{requestId}")]
        public async Task<IActionResult> GetAllRelevatDocument(int requestId)
        {
            try
            {
                var httpContext = HttpContext;
                RelevantDocument relevantDocuments = await _travelDocumentFileDataService.GetAllRelevantDocuments(requestId, httpContext);
                return Ok(relevantDocuments);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting travel documents: {ex.Message}");
            }
        }

        [HttpGet("{fileType}/searchbyEmployee/{employeeName}")]
        public async Task<IActionResult> GetDocumentsByEmployeeName(string fileType, string employeeName, int filterId)
        {
            try
            {
                var httpContext = HttpContext;
                IEnumerable<TravelDocumentViewModel> filteredDocuments = await _travelDocumentFileDataService.GetTravelDocumentByEmployeeName(fileType, employeeName, filterId, httpContext);
                return Ok(filteredDocuments);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting travel documents: {ex.Message}");
            }
        }
    }
}
