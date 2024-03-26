using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.FileTypeService;
using XtramileBackend.Services.TravelDocumentFileData;

namespace XtramileBackend.Controllers.TravelDocFileData
{
    [EnableCors("AllowAngularDev")]
    [Route("api/traveldocumentfile")]
    [ApiController]
    public class TravelDocFileDataController : ControllerBase
    {
        private readonly ITravelDocumentFileDataService _travelDocumentFileDataService;
        public TravelDocFileDataController(ITravelDocumentFileDataService travelDocumentFileDataService) {
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
                TravelDocumentFileDataModel travelDocuments = await _travelDocumentFileDataService.AddTravelDocumentFileAsync(travelDocFile,httpContext);
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
        public async Task<IActionResult> GetTravelDocumentsFilteredByFileType(string fileType)
        {
            try
            {
                var httpContext = HttpContext;
                IEnumerable<TravelDocumentViewModel> travelDocumentFiles = await _travelDocumentFileDataService.GetFilteredDocumentsOnTAScreen(fileType,httpContext);
                return Ok(travelDocumentFiles);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting travel documents: {ex.Message}");
            }
        }

        [HttpGet("expiredDocuments/{fileType}")]
        public async Task<IActionResult> GetExpiredDocuments(string fileType)
        {
            try
            {
                var httpContext = HttpContext;
                IEnumerable<TravelDocumentViewModel> travelDocumentFiles = await _travelDocumentFileDataService.GetExpiredDocuments(fileType, httpContext);
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
                return Ok("File Deleted Successfully");
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting travel document: {ex.Message}");
            }
        }

        [HttpGet("validDocuments/{fileType}")]
        public async Task<IActionResult> GetValidDocuments(string fileType)
        {
            try
            {
                var httpContext = HttpContext;
                IEnumerable<TravelDocumentViewModel> travelDocumentFiles = await _travelDocumentFileDataService.GetValidDocuments(fileType, httpContext);
                return Ok(travelDocumentFiles);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting travel documents: {ex.Message}");
            }
        }
    }
}
