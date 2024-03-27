using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Services.TravelAdminService;

namespace XtramileBackend.Controllers.TravelAdminControllers
{
    [Route("api/traveladmin")]
    [ApiController]
    /*[Authorize("Manager")]*/
    public class TravelAdminController : ControllerBase
    {
        private readonly ITravelAdminService _travelAdminService;
        public TravelAdminController(ITravelAdminService travelAdminService) {
            _travelAdminService = travelAdminService;
        }

        [HttpGet("ongoing")]
        public async Task<IActionResult> GetOnGoingTravel([FromQuery]int pageIndex=1, int pageSize=10)
        {
            try
            {
                var onGoingTravelData = await _travelAdminService.OnGoingTravel(pageIndex, pageSize);
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
        [HttpGet("requestsView/{primaryStatusCode}/{secondaryStatusCode}")]
        public async Task<IActionResult> viewTravelRequestByCode(string primaryStatusCode, string secondaryStatusCode,int pageSize=10, int pageIndex=1)
        {
            try
            {
                RequestTableViewTravelAdminPaged requestData = await _travelAdminService.GetTravelRequests(primaryStatusCode,secondaryStatusCode,pageSize,pageIndex);
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

        //to return generate an excel sheet based on the input monthName
        [HttpGet("generatemontlhlymodecountreport")]
        public async Task<IActionResult> GenerateModeCountFromMonthReport(string monthName)
        {
            try
            {   
                byte[] excelData = await _travelAdminService.GenerateModeCountFromMonthReport(monthName);
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "month_mode_count_report.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
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

        //to return generate an excel sheet based on the input projectId
        [HttpGet("generateprojectmodecount")]

        public async Task<IActionResult> GenerateModeFromProjectId(int projectId)
        {
            try
            {
                byte[] excelData = await _travelAdminService.GenerateModeCountFromProjectIdExcelReport(projectId);
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "project_mode_count_report.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        //to get all requests completed in respective months
        [HttpGet("requestsbymonth")]
        public async Task<ActionResult<Dictionary<string, int>>> GetRequestsByMonth()
        {
            try
            {
                var requestsByMonth = await _travelAdminService.GetRequestsByMonth();
                return Ok(requestsByMonth);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        //to genrate a report of request related details of the input month
       [HttpGet("generateReport/{monthName}/{year}")]
        public async Task<IActionResult> GenerateReportForMonth(string monthName,int year)
        {
            try
            {
                var excelBytes = await _travelAdminService.GenerateReportForMonthAndYear( monthName,year);

                if (excelBytes == null)
                    return NotFound();

                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Requests.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("selectedoption/{reqId}")]
        public async Task<ActionResult<int?>> GetSelectedTravelOptionFromEmployee(int reqId)
        {
            try
            {
                var optionId = await _travelAdminService.GetSelectedTravelOptionFromEmployee(reqId);


                return Ok(optionId);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        //to get the compleled trips of a given empId
        [HttpGet("completedtrips/{empId}")]
        public async Task<IActionResult> GetCompletedTrips(int empId)
        {
            try
            {
                var completedTrips = await _travelAdminService.GetCompletedTrips(empId);
                if (completedTrips == null)
                {
                    return StatusCode(500, "An error occurred while processing your request.");
                }
                return Ok(completedTrips);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("Closed")]
        public async Task<IActionResult> GetClosedTravel([FromQuery] int pageIndex=1, int pageSize=10)
        {
            try
            {
                var onClosedTravelData = await _travelAdminService.ClosedTravel(pageIndex,pageSize);
                return Ok(onClosedTravelData);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting on closed travel requests: {ex.Message}");
            }
        }
    }
}
