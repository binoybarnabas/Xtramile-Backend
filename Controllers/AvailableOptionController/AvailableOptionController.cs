using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.AvailableOptionService;
using XtramileBackend.Services.FileMetaDataService;
using XtramileBackend.Services.FileTypeService;
using XtramileBackend.Services.RequestService;

namespace XtramileBackend.Controllers.AvailableOptionControllers
{

    [EnableCors("AllowAngularDev")]
    [Route("api/availableoptions")]
    [ApiController]
    public class AvailableOptionController : ControllerBase
    {
        private readonly IAvailableOptionServices _availableOptionServices;

        private readonly IFileTypeServices _fileTypeServices;

        private readonly IFileMetaDataService _fileMetaDataServices;

        private readonly IRequestServices _requestServices;


        public AvailableOptionController(IRequestServices requestServices, IAvailableOptionServices availableOptionServices, IFileTypeServices fileTypeServices, IFileMetaDataService fileMetaDataServices)
        {
            _availableOptionServices = availableOptionServices;
            _fileTypeServices = fileTypeServices;
            _fileMetaDataServices = fileMetaDataServices;
            _requestServices = requestServices;

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


        //Add New Travel Option - Files Not Received from front end
        [HttpPost("addoption")]
        public async Task<IActionResult> AddTravelOptionAsync([FromForm] TravelOptionViewModel travelOption)
        {
           /* Console.WriteLine("FormDatas" + travelOption.RequestId);
            Console.WriteLine("FormDatas" + travelOption.GetType);
            Console.WriteLine("FormDatas" + travelOption.OptionFile.Name);*/
            try
            {
                //Handling text data of travel request
                var tblTravelOption = new TBL_TRAVEL_OPTION
                {
                    RequestId = int.Parse( travelOption.RequestId),
                    Description = travelOption.Description,

                };

/*                await _availableOptionServices.AddNewTravelOptionAsync(tblTravelOption);
*/                Console.WriteLine(HttpContext.Request.Form.Files.Count);

                int optionId = await _availableOptionServices.AddNewTravelOptionAsync(tblTravelOption);
                // Now you can access the optionId

                // Check if files are attached and handle them
                if (HttpContext.Request.Form.Files != null && HttpContext.Request.Form.Files.Count > 0)
                {
                    foreach (var file in HttpContext.Request.Form.Files)
                    {
                        string randomCode = _requestServices.GenerateRandomCode(int.Parse(travelOption.RequestId));

                        // Renaming the file using REQCODE
                        var fileName = $"{randomCode}{file.FileName}";

                        // Define the target folder
                        var targetFolder = "Uploads/RequestFiles/TravelOptions";
                        if (!Directory.Exists(targetFolder))
                        {
                            // Create directory
                            try
                            {
                                Directory.CreateDirectory(targetFolder);
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


                        /*                        var filePath = Path.Combine(targetFolder, fileName);
                        */
                        var filePath = Path.Combine(targetFolder, fileName).Replace("\\", "/");

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Get Extension of received file
                        string fileExtension = Path.GetExtension(filePath);

                        // Get file type id based on the file extension of received file
                        int fileTypeId = await _fileTypeServices.GetFileTypeIdByExtensionAsync(fileExtension.Substring(1));

                        // To save the file meta data in TBL_FILE_METADATA
                        var fileMetaData = new TBL_FILE_METADATA
                        {
                            RequestId = int.Parse(travelOption.RequestId),
                            FileName = fileName,
                            FilePath = targetFolder,
                            Description = file.Name, // Assuming file.Name is appropriate for description
                            FileTypeId = fileTypeId,
                            CreatedOn = DateTime.Now,
                            CreatedBy = 1,
                        };

                        // Adding files meta data
                        await _fileMetaDataServices.AddFileMetaDataAsync(fileMetaData);

                        int fileId = await _fileMetaDataServices.GetFileIdByFileNameAsync(fileName);

                        await _availableOptionServices.UpdateFileIdOfOptionAsync(fileId, optionId);

                    }

                }

                return Ok("Option Added successfully:-");
            }
            catch (Exception ex)
            {
                // Log and handle any exceptions
                Console.WriteLine($"Error processing request: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }



        //Get Travel Options By Req ID
        [HttpGet("get_travel_options_by_request_id/{reqId}")]
        public async Task<IActionResult> GetTravelOptionsByReqIdAsync(int reqId)
        {
            try
            {
                IEnumerable<TBL_TRAVEL_OPTION> travelOptionsData = await _availableOptionServices.GetTravelOptionsByRequestIdAsync(reqId,false);

                //var OptionFilePath = await _fileMetaDataServices.GetFilePathByRequestIdAndDescriptionAsync(reqId, "OptionFile");

                var travelOptionsViewDataList = new List<TravelOptionViewModel>();

                foreach (var travelOption in travelOptionsData)
                {
                    var travelOptionsViewData = new TravelOptionViewModel();
                    travelOptionsViewData.OptionId = travelOption.OptionId;
                    travelOptionsViewData.RequestId = travelOption.RequestId.ToString();
                    travelOptionsViewData.Description = travelOption.Description;
                
                    int? fileId = travelOption.FileId; // Assuming travelOption.FileId is int?
                    //Get file path by fileID--bug
                    var fileData = await _fileMetaDataServices.GetFileMetaDataById(fileId.Value);

                    string filePath = fileData.FilePath;
                    string fileName = fileData.FileName;

                    // travelOptionsViewData.OptionFileURL = HttpUtility.UrlEncode( OptionFilePath != null ? $"D:/SPECIALIZATION/XtraMileProject/BackEndV2/Xtramile-Backend/{OptionFilePath}" : "file_not_found");
                    var urlRequest = HttpContext.Request;
                    travelOptionsViewData.OptionFileURL = filePath != null ? $"{urlRequest.Scheme}://{urlRequest.Host}/{filePath}/{Uri.EscapeDataString(fileName)}" : "file_not_found";

                    travelOptionsViewDataList.Add(travelOptionsViewData);
                }

                return Ok(travelOptionsViewDataList);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting available options: {ex.Message}");
            }
        }


        //add new option in the form of text
        [HttpPost("addtextoption")]
        public async Task<IActionResult> AddTextsAsTravelAvailableOption([FromBody] AvailableOption availableOption)
        {

            try {
                string response = await _availableOptionServices.AddAvailableTextOptionAsync(availableOption);
                return Ok(response);
            }
            catch(Exception ex)
            {  
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding available options as texts: {ex.Message}");

            }

        }

        [HttpGet("gettextoptions/${requestId}")]
        public async Task<IActionResult> AddTextsAsTravelAvailableOption(int requestId)
        {
            try
            {
                IEnumerable< TBL_TRAVEL_OPTION> travelOptions = await _availableOptionServices.GetTravelOptionsByRequestIdAsync(requestId,true);
                return Ok(travelOptions);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding available options as texts: {ex.Message}");

            }

        }

        [HttpDelete("deleteTravelOptions")]
        public async Task<IActionResult> DeleteTravelOptions(int[] fileIds)
        {
            try
            {
                await _availableOptionServices.DeleteTravelOptions(fileIds);
                return Ok("Options Deleted Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting travel options : {ex.Message}");

            }
        }

    }
}
