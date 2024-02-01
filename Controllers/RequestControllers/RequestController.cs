using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Models.APIModels;

using XtramileBackend.Services.RequestService;
using Microsoft.EntityFrameworkCore;
using XtramileBackend.Services.RequestStatusService;

namespace XtramileBackend.Controllers.RequestControllers
{
    [EnableCors("AllowAngularDev")]
    [Route("api/request")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestServices _requestServices;

        private readonly IRequestStatusServices _requestStatusServices;

        public RequestController(IRequestServices requestServices, IRequestStatusServices requestStatusServices)
        {
            _requestServices = requestServices;
            _requestStatusServices = requestStatusServices;
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


        /*        //New Travel Requests
                [HttpPost("add")]
                public async Task<IActionResult> AddRequestAsync([FromForm] TravelRequestViewModel request)
                {

                    Console.WriteLine("==============================================================================================");
                    Console.WriteLine(request);

                    Console.WriteLine("==============================================================================================");

                        // Your logic to handle form data (model) here

                        // Check if files are attached and handle them
                        if (HttpContext.Request.Form.Files != null && HttpContext.Request.Form.Files.Count > 0)
                        {
                            foreach (var file in HttpContext.Request.Form.Files)
                            {
                                // Handle each file, for example, save it to the server
                                // Note: You may need to adjust the file handling logic based on your requirements
                                // For simplicity, the file is saved to wwwroot/uploads folder with a unique filename
                                var fileName = $"{System.Guid.NewGuid()}_{file.FileName}";
                                var filePath = System.IO.Path.Combine("Uploads/EmployeeUploads/IDCards", fileName);

                                using (var stream = System.IO.File.Create(filePath))
                                {
                                    await file.CopyToAsync(stream);
                                }

                                // Add logic to save the file path or other details in your database
                            }
                        }

                        // Your remaining logic for handling the request

                        return Ok("Request submitted successfully");




                }

        */

        [HttpPost("add")]
        public async Task<IActionResult> AddRequestAsync([FromForm] TravelRequestViewModel request)
        {

            try
            {
                //Handling the request data

                int empId = int.Parse(request.CreatedBy);

                string RequestCode ="REQ"+_requestServices.GenerateRandomCode(empId);

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

                await _requestServices.AddRequestAsync(tblRequest);

                var requestStatus = new TBL_REQ_APPROVE
                {

                    //update with req id
                    RequestId = 1,
                    EmpId = int.Parse(request.CreatedBy),
                    PrimaryStatusId = 1,
                    date = DateTime.Now,
                    SecondaryStatusId = 2

                };


                //Update mapping tables
                await _requestStatusServices.AddRequestStatusAsync(requestStatus);
                

                // Check if files are attached and handle them
                if (HttpContext.Request.Form.Files != null && HttpContext.Request.Form.Files.Count > 0)
                {
                    foreach (var file in HttpContext.Request.Form.Files)
                    {
                        
                        //var fileName = $"{System.Guid.NewGuid()}_{file.FileName}";

                        var fileName = $"{RequestCode}{file.FileName}";

                        var filePath = System.IO.Path.Combine("Uploads/EmployeeUploads/IDCards", fileName);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Add logic to save the file path or other details in your database
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
