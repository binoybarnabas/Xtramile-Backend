using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Services.TravelAdminService;

namespace XtramileBackend.Controllers.TravelAdminControllers
{
    [Route("api/traveladmin")]
    [ApiController]
    [Authorize("Manager")]
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
        public async Task<IActionResult> GetIncomingRequests(int pageIndex=1, int pageSize = 10)
        {
            try
            {
                var incomingRequestData = await _travelAdminService.GetIncomingRequests(pageIndex,pageSize);
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

        [HttpGet("travel/request/{requestID}")]
        public async Task<IActionResult> GetTravelRequest(int requestID)
        {
            try
            {
                // Call the service method to retrieve ongoing travel request details for employees reporting to the specified manager
                TravelRequestEmployeeViewModel requestData = await _travelAdminService.GetEmployeeRequestDetail(requestID);
                // Return a 200 OK response with the retrieved ongoing travel request details
                return Ok(requestData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting ongoing travel request details: {ex.Message}");
            }
        }

        /// <summary>
        /// to sort incoming request based on the employeename and date of request
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="employeeName"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet("incomingrequest/sort")]
        public async Task<IActionResult> GetIncomingRequestsSorted([FromQuery] int pageIndex, int pageSize, bool employeeName, bool date)
        {
            try
            {
                var travelRequests = await _travelAdminService.GetIncomingRequestsSorted(pageIndex, pageSize, employeeName, date);
                return Ok(travelRequests);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured while getting incoming requests sorted: {ex.Message}");
                throw;
            }

        }


        //api for appbar
        // Get employee requests for a specific date based on managerId and date
        [HttpGet("date")]
        public async Task<IActionResult> GetEmployeeRequestByDateAsync([FromQuery]string date)
        {
            var empRequests = await _travelAdminService.GetEmployeeRequestsByDateAsync(date);
            return Ok(empRequests);
        }

        // Get employee requests sorted by employee name 
        [HttpGet("search/employeename")]
        public async Task<IActionResult> GetEmployeeRequestSortEmployeeNameAsync([FromQuery] string employeeName)
        {
            var empRequests = await _travelAdminService.GetEmployeeRequestsByEmployeeNameAsync(employeeName);
            return Ok(empRequests);
        }
    }
}
