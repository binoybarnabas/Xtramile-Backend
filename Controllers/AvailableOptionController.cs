using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Org.BouncyCastle.Ocsp;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.AvailableOptionService;
using XtramileBackend.Services.FileMetaDataService;
using XtramileBackend.Services.FileTypeService;
using XtramileBackend.Services.RequestService;
using XtramileBackend.Services.RequestStatusService;
using AvailableOption = XtramileBackend.Models.EntityModels.AvailableOption;
using TravelOption = XtramileBackend.Models.EntityModels.TravelOption;

namespace XtramileBackend.Controllers
{

    [EnableCors("AllowAngularDev")]
    [Route("api/availableoptions")]
    [ApiController]
    public class AvailableOptionController : ControllerBase
    {
        private readonly IAvailableOptionServices _availableOptionServices;

        private readonly IFileMetaDataService _fileMetaDataServices;
        
        public AvailableOptionController(IAvailableOptionServices availableOptionServices, IFileMetaDataService fileMetaDataServices)
        {
            _availableOptionServices = availableOptionServices;
            
            _fileMetaDataServices = fileMetaDataServices;
            
        }

        [HttpGet("traveloptions")]
        public async Task<IActionResult> GetAvailableOptionsAsync()
        {
            try
            {
                IEnumerable<AvailableOption> availableOptionData = await _availableOptionServices.GetAvailableOptionsAsync();
                return Ok(availableOptionData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting available options: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAvailableOptionAsync([FromBody] AvailableOption availableOption)
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

        [HttpPost("addoption")]
        public async Task<IActionResult> AddTravelAvailableOption([FromForm] TravelOptionAPI travelOption)
        {
            try {
                var httpContext = HttpContext;
                Console.WriteLine(httpContext);
                await _availableOptionServices.AddTravelAvailableOption(travelOption, httpContext);
                return Ok("Option Added successfully:-");
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding an available option: {ex.Message}");
            }
           
        }


        //Get Travel Options By Req ID
        [HttpGet("get_travel_options_by_request_id/{reqId}")]
        public async Task<IActionResult> GetTravelOptionsByReqIdAsync(int reqId)
        {
            try
            {
                IEnumerable<TravelOption> travelOptionsData = await _availableOptionServices.GetTravelOptionsByRequestIdAsync(reqId, false);

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

        [HttpGet("gettextoptions/${requestId}")]
        public async Task<IActionResult> AddTextsAsTravelAvailableOption(int requestId)
        {
            try
            {
                IEnumerable<TravelOption> travelOptions = await _availableOptionServices.GetTravelOptionsByRequestIdAsync(requestId, true);
                return Ok(travelOptions);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding available options as texts: {ex.Message}");

            }

        }

        [HttpDelete("deleteTravelOptions")]
        public async Task<IActionResult> DeleteTravelOptions([FromBody] int[] fileIds)
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

        [HttpPatch("updateSelectedTravelOption")]
        public async Task<IActionResult> UpdateSelectedTravelOption(TravelOptionMap travelOption)
        {
            try
            {
                await _availableOptionServices.UpdateTravelOptionSelected(travelOption);
                return Ok("Updated Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while updating travel options : {ex.Message}");
            }
        }

    }
}
