using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Models.APIModels;

using XtramileBackend.Services.RequestService;
using Microsoft.EntityFrameworkCore;
using XtramileBackend.Services.RequestStatusService;
using XtramileBackend.Services.StatusService;
using XtramileBackend.Services.FileTypeService;
using XtramileBackend.Repositories.FileMetaDataRepository;
using XtramileBackend.Services.FileMetaDataService;

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

        public RequestController(IRequestServices requestServices, IRequestStatusServices requestStatusServices, 
            
            IStatusServices statusServices, IFileTypeServices fileTypeServices, IFileMetaDataService fileMetaDataServices)
        {
            _requestServices = requestServices;
            _requestStatusServices = requestStatusServices;
            _statusServices = statusServices;
            _fileTypeServices = fileTypeServices;
            _fileMetaDataServices = fileMetaDataServices;
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
                string randomCode =  _requestServices.GenerateRandomCode(empId);

                string RequestCode = "REQ" + randomCode;


                //Handling text data of travel request
                var tblRequest = new TBL_REQUEST
                {
                    RequestCode = RequestCode,
                    TravelTypeId = 1,
                    TripPurpose = request.TripPurpose,
                    DepartureDate = DateTime.Parse(request.DepartureDate),
                    ReturnDate = DateTime.Parse(request.ReturnDate),
                    SourceCityZipCode = request.SourceCityZipCode,
                    DestinationCityZipCode = request.DestinationCityZipCode,
                    SourceCity = request.SourceCity,
                    DestinationCity = request.DestinationCity,
                    SourceState = request.SourceState,
                    DestinationState = request.DestinationState,
                    SourceCountry = request.SourceCountry,
                    DestinationCountry = request.DestinationCountry,
                    CabRequired = request.CabRequired,
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
                                      var filePath = Path.Combine(targetFolder, fileName);

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








    }
}
