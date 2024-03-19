using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Web;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.EmployeeService;
using XtramileBackend.Services.FileMetaDataService;
using XtramileBackend.Services.FileTypeService;
using XtramileBackend.Services.ProjectService;
using XtramileBackend.Services.RequestService;
using XtramileBackend.Services.RequestStatusService;
using XtramileBackend.Services.StatusService;
using XtramileBackend.Services.TravelModeService;

namespace XtramileBackend.Controllers.RequestControllers
{
    [EnableCors("AllowAngularDev")]
    [Route("api/request")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestServices _requestServices;

        private readonly IRequestStatusServices _requestStatusServices;

        private readonly IStatusServices _statusServices;

        private readonly IFileTypeServices _fileTypeServices;

        private readonly IFileMetaDataService _fileMetaDataServices;

        private readonly IEmployeeServices _employeeService;


        private readonly IProjectServices _projectServices;

        private readonly ITravelModeService _travelModeService;


        public RequestController(IRequestServices requestServices, IRequestStatusServices requestStatusServices,

            IStatusServices statusServices, IFileTypeServices fileTypeServices, IFileMetaDataService fileMetaDataServices,

            IEmployeeServices employeeServices, IProjectServices projectServices, ITravelModeService travelModeService


            )
        {
            _requestServices = requestServices;
            _requestStatusServices = requestStatusServices;
            _statusServices = statusServices;
            _fileTypeServices = fileTypeServices;
            _fileMetaDataServices = fileMetaDataServices;
            _employeeService = employeeServices;
            _projectServices = projectServices;
            _travelModeService = travelModeService;
        }

        [HttpGet("requests")]
        public async Task<IActionResult> GetAllRequestAsync()
        {
            try
            {
                IEnumerable<TBL_REQUEST> request = await _requestServices.GetAllRequestAsync();
                return Ok(request);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting request: {ex.Message}");
            }
        }





        /// <summary>
        /// Controller to handle new travel request
        /// Form input contains mutiple input types including files
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> AddRequestAsync([FromForm] TravelRequestViewModel request)
        {
            try
            {

                int empId = int.Parse(request.CreatedBy);

                //Request Code Generation
                string randomCode = _requestServices.GenerateRandomCode(empId);

                string RequestCode = "REQ" + randomCode;

                string travelType = "";

                DateTime? returnDate = null; // Declare returnDate as nullable DateTime

                if (!string.IsNullOrEmpty(request.ReturnDate))
                {
                    returnDate = DateTime.Parse(request.ReturnDate); // Parse returnDate if it's not null or empty
                }
                if (request.TravelType.CompareTo("domestic") == 0) travelType = "Domestic";
                else if (request.TravelType.CompareTo("international") == 0) travelType = "International";


                //Handling text data of travel request
                var tblRequest = new TBL_REQUEST
                {

                    RequestCode = RequestCode,

                    ProjectId = Int32.Parse(request.ProjectId),

                    TripType = request.TripType,

                    TravelModeId = Int32.Parse(request.TravelModeId),

                    TravelType = travelType,

                    TripPurpose = request.TripPurpose,

                    DepartureDate = DateTime.Parse(request.DepartureDate),

                    ReturnDate = returnDate,

                    SourceCity = request.SourceCity,

                    DestinationCity = request.DestinationCity,

                    SourceCountry = request.SourceCountry,

                    DestinationCountry = request.DestinationCountry,

                    CabRequired = request.CabRequired,

                    PrefPickUpTime = request.PrefPickUpTime,

                    AccommodationRequired = request.AccommodationRequired,

                    PrefDepartureTime = request.PrefDepartureTime,

                    AdditionalComments = request.AdditionalComments,

                    PriorityId = null,
                    PerdiemId = null,
                    CreatedOn = DateTime.Now,
                    CreatedBy = int.Parse(request.CreatedBy),
                    ModifiedBy = null,
                    ModifiedOn = null,

                };

                //Adding text data to the TBL_REQUEST Entity
                await _requestServices.AddRequestAsync(tblRequest);


                //Getting the requestID after insertion
                int requestId = await _requestServices.GetRequestIdByRequestCode(RequestCode);


                //Getting the statusID from status table
                int primaryStatusId = await _statusServices.GetStatusIdByStatusCodeAsync("OP");
                int secondaryStatusId = await _statusServices.GetStatusIdByStatusCodeAsync("PE");


                //Object for updating status of request
                var requestStatus = new TBL_REQ_APPROVE
                {

                    RequestId = requestId,
                    EmpId = int.Parse(request.CreatedBy),
                    PrimaryStatusId = primaryStatusId,
                    date = DateTime.Now,
                    SecondaryStatusId = secondaryStatusId

                };


                //Updating Requesting Status
                await _requestStatusServices.AddRequestStatusAsync(requestStatus);


                // Check if files are attached and handle them
                if (HttpContext.Request.Form.Files != null && HttpContext.Request.Form.Files.Count > 0)
                {
                    foreach (var file in HttpContext.Request.Form.Files)
                    {
                        //Renaming the file using REQCODE
                        var fileName = $"{RequestCode}{file.FileName}";

                        // Get the form field name (key name)
                        var keyName = file.Name;

                        // Mapping between form field names and target folders
                        var folderMapping = new Dictionary<string, string>
                                {
                                      { "idCardAttachment", "Uploads/ProfileFiles/IdCards" },
                                      { "travelAuthorizationEmailCapture", "Uploads/RequestFiles/TravelAuthorizationEmails" },
                                      { "passportAttachment", "Uploads/ProfileFiles/Passports" }

                                };


                        // Determine the target folder based on the form field name
                        if (folderMapping.TryGetValue(keyName, out var targetFolder))
                        {
                            // var filePath = Path.Combine(targetFolder, fileName);
                            var filePath = Path.Combine(targetFolder, fileName).Replace("\\", "/");

                            using (var stream = System.IO.File.Create(filePath))
                            {
                                await file.CopyToAsync(stream);
                            }

                            //Get Extension of received file
                            string fileExtension = Path.GetExtension(filePath);

                            //Get file type id based on the file extension of received file
                            int fileTypeId = await _fileTypeServices.GetFileTypeIdByExtensionAsync(fileExtension.Substring(1));


                            // To save the file meta data in TBL_FILE_METADATA
                            var fileMetaData = new TBL_FILE_METADATA
                            {
                                RequestId = requestId,
                                FileName = fileName,
                                FilePath = filePath,
                                Description = keyName,
                                FileTypeId = fileTypeId,
                                CreatedOn = DateTime.Now,
                                CreatedBy = int.Parse(request.CreatedBy),

                            };

                            //Adding files meta data
                            await _fileMetaDataServices.AddFileMetaDataAsync(fileMetaData);

                        }
                        else
                        {     //FOR REFERENCE!  
                              // Handle the case where there is no specific folder mapping for the form field name
                              // You can choose to skip the file, move it to a default folder, or handle it in another way
                        }

                        //for ends
                    }

                }

                return Ok("Request submitted successfully:-");
            }
            catch (Exception ex)
            {
                // Log and handle any exceptions
                Console.WriteLine($"Error processing request: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }



        //Get Request By Id
        [HttpGet("getbyid/{reqId}")]
        public async Task<IActionResult> GetRequestByIdAsync(int reqId)
        {
            try
            {



                TBL_REQUEST request = await _requestServices.GetRequestById(reqId);


                //Get Employee Name
                EmployeeInfo EmployeeData = await _employeeService.GetEmployeeInfo(request.CreatedBy);

                //Get Project Code
                string ProjectCode = await _projectServices.GetProjectCodeByProjectIdAsync(request.ProjectId);

                //Get Status
                string Status = await _statusServices.GetPrimaryStatusByRequestIdAsync(request.RequestId);

                //Get Mode
                string TravelMode = await _travelModeService.GetTravelModeByIdAsync(request.TravelModeId);


                //Get Files
                // Fetch file paths from tbl_file_metadata
                var PassportFilePath = await _fileMetaDataServices.GetFilePathByRequestIdAndDescriptionAsync(reqId, "passportAttachment");
                var TravelAuthMailFilePath = await _fileMetaDataServices.GetFilePathByRequestIdAndDescriptionAsync(reqId, "travelAuthorizationEmailCapture");
                /*
                                Console.Write("Path"+PassportFilePath);
                                Console.WriteLine(reqId);*/

                // Construct file URLs
                var urlRequest = HttpContext.Request;

                var passportFileUrl = PassportFilePath != null ? $"{urlRequest.Scheme}://{urlRequest.Host}/{Uri.EscapeUriString(PassportFilePath)}" : "404_file_not_found";
                var travelAuthMailFileUrl = TravelAuthMailFilePath != null ? $"{urlRequest.Scheme}://{urlRequest.Host}/{Uri.EscapeUriString(TravelAuthMailFilePath)}".Replace(" ", "%20") : "file_not_found";

                //string encodedPassportUrl = HttpUtility.UrlEncode(passportFileUrl);
                //string encodedTravelAuthMailUrl = HttpUtility.UrlEncode(travelAuthMailFileUrl);

                /*                // Construct file URLs
                                var passportFileUrl = PassportFilePath != null ? $"D:\\SPECIALIZATION\\XtraMile Project\\Back End V2\\Xtramile-Backend\\Uploads\\RequestFiles\\TravelAuthorizationEmails\\REQ36015why2.png" : "file_not_found";
                *//*                var travelAuthMailFileUrl = TravelAuthMailFilePath != null ? $"D:\\SPECIALIZATION\\XtraMile Project\\Back End V2\\Xtramile-Backend\\{TravelAuthMailFilePath}" : "file_not_found";
                */

                var travelrequestViewData = new TravelRequestViewModel
                {
                    EmployeeName = EmployeeData.FirstName,
                    ProjectCode = ProjectCode,
                    SourceCity = request.SourceCity,
                    SourceCountry = request.SourceCountry,
                    DestinationCity = request.DestinationCity,
                    DestinationCountry = request.DestinationCountry,
                    DepartureDate = request.DepartureDate.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    ReturnDate = request.ReturnDate?.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    RequestCode = request.RequestCode,
                    TravelModeId = TravelMode,
                    PrimaryStatus = Status,
                    //PassportFileUrl = HttpUtility.UrlEncode(passportFileUrl),
                    //TravelAuthMailFileUrl = HttpUtility.UrlEncode(passportFileUrl)
                    PassportFileUrl = passportFileUrl,
                    TravelAuthMailFileUrl = travelAuthMailFileUrl

                };


                return Ok(travelrequestViewData);

            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting request: {ex.Message}");
            }
        }

        [HttpGet("request/reason/{requestId}")]
        public async Task<IActionResult> GetReasonDescriptionByRequestId(int requestId)
        {
            try
            {
                string reasonDescription = await _requestServices.GetReasonDescriptionByRequestId(requestId);
                return Ok(reasonDescription);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting reason description: {ex.Message}");
            }
        }

    }
}
