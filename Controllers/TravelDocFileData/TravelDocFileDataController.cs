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
        private readonly IFileTypeServices _fileTypeServices;
        public TravelDocFileDataController(ITravelDocumentFileDataService travelDocumentFileDataService, IFileTypeServices fileTypeServices) {
            _travelDocumentFileDataService = travelDocumentFileDataService;
            _fileTypeServices = fileTypeServices;
        }

        [HttpGet("traveldocumentfiles")]
        public async Task<IActionResult> GetTravelDocumentFilesAsync()
        {
            try
            {
                IEnumerable<TravelDocumentFileDataModel> travelDocumentFiles = await _travelDocumentFileDataService.GetTravelDocumentFileDatasAsync();
                return Ok(travelDocumentFiles);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting travel documents: {ex.Message}");
            }
        }

        [HttpGet("traveldocumentfiles/{id}")]
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
                string uploadsDirectory = "";
                string fileName = "";
                string filePath = "";
                int fileTypeId = 0;

                if (string.Compare(travelDocFile.TravelDocType, "ID Card") == 0)
                    uploadsDirectory = "Uploads/TravelDocuments/IdCards";
                else if (string.Compare(travelDocFile.TravelDocType, "Passport") == 0)
                    uploadsDirectory = "Uploads/TravelDocuments/Passports";
                else if (string.Compare(travelDocFile.TravelDocType, "Visa") == 0)
                    uploadsDirectory = "Uploads/TravelDocuments/Visas";

                if (!Directory.Exists(uploadsDirectory))
                {
                    // Create directory
                    try
                    {
                        Directory.CreateDirectory(uploadsDirectory);
                        Console.WriteLine("Directory created successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error creating directory: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Directory already exists.");
                }

                if (HttpContext.Request.Form.Files != null)
                {
                    var file = HttpContext.Request.Form.Files[0];
                    fileName = $"{travelDocFile.TravelDocType}_{travelDocFile.UploadedBy}_{file.FileName}";
                    filePath = uploadsDirectory;
                    string fileExtension = Path.GetExtension(filePath);
                    fileTypeId = await _fileTypeServices.GetFileTypeIdByExtensionAsync(fileExtension.Substring(1));
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                TravelDocumentFileDataModel travelDocuments = new TravelDocumentFileDataModel
                {
                    TravelDocType = travelDocFile.TravelDocType,
                    FileName = fileName,
                    FilePath = uploadsDirectory,
                    FileTypeId = fileTypeId,
                    UploadedBy = travelDocFile.UploadedBy,
                    Country = travelDocFile.Country,
                    ExpiryDate = travelDocFile.ExpiryDate,
                    DocId = travelDocFile.DocId.ToUpper(),
                };

                await _travelDocumentFileDataService.AddTravelDocumentFileAsync(travelDocuments);
                return CreatedAtAction(nameof(GetTravelDocumentFilesById), new { id = travelDocuments.TravelDocFileId }, travelDocuments);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding the travel document: {ex.Message}");
            }

        }
    }
}
