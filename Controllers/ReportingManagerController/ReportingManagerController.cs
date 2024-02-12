using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.ManagerService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Services.ManagerService;
using Microsoft.AspNetCore.Authorization;

// Controller for handling reporting manager related actions
    namespace XtramileBackend.Controllers.ReportingManagerController
    {

        [EnableCors("AllowAngularDev")]
        [Route("api/reportingmanager")]
        [Authorize("Manager")]
        [ApiController]
        public class ReportingManagerController : ControllerBase
        {
            public readonly IReportingManagerService _reportingManagerService;

            // Constructor to inject the Reporting Manager Service
            public ReportingManagerController(IReportingManagerService reportingManagerService)
            {
                _reportingManagerService = reportingManagerService;
            }

            // Get employee requests based on managerId
            [HttpGet("request")]
            public async Task<IActionResult> GetEmployeeRequestAsync([FromQuery] int managerId,int offset=1,int pageSize=10)
            {
                var empRequests = await _reportingManagerService.GetEmployeeRequestsAsync(managerId,offset,pageSize);
                return Ok(empRequests);
            }

            // Get employee requests for a specific date based on managerId and date
            [HttpGet("date")]
            public async Task<IActionResult> GetEmployeeRequestByDateAsync([FromQuery] int managerId, string date, int offset = 1, int pageSize = 10)
            {
                var empRequests = await _reportingManagerService.GetEmployeeRequestsByDateAsync(managerId, date,offset,pageSize);
                return Ok(empRequests);
            }

            // Get employee requests sorted by date based on managerId
            [HttpGet("sort/date")]
            public async Task<IActionResult> GetEmployeeRequestSortByDateAsync([FromQuery] int managerId, int offset = 1, int pageSize = 10)
            {
                var empRequests = await _reportingManagerService.GetEmployeeRequestsSortByDateAsync(managerId,offset,pageSize);
                return Ok(empRequests);
            }

            // Get employee requests sorted by email based on managerId
            [HttpGet("sort/employeename")]
            public async Task<IActionResult> GetEmployeeRequestSortEmployeeNameAsync([FromQuery] int managerId, int offset = 1, int pageSize = 10)
            {
                var empRequests = await _reportingManagerService.GetEmployeeRequestsSortByEmployeeNameAsync(managerId, offset, pageSize);
                return Ok(empRequests);
            }

            // Get employee requests sorted by email based on managerId
            [HttpGet("search/employeename")]
            public async Task<IActionResult> GetEmployeeRequestByEmployeeNameAsync([FromQuery] int managerId,string employeeName, int offset = 1, int pageSize = 10)
            {
                var empRequests = await _reportingManagerService.GetEmployeeRequestsByEmployeeNameAsync(managerId,employeeName,offset,pageSize);
                return Ok(empRequests);
            }

            /// <summary>
            /// Retrieves ongoing travel request details for employees reporting to a specific manager.
            /// </summary>
            /// <param name="managerId">The ID of the reporting manager.</param>
            /// <returns>
            /// 200 OK response with a collection of ongoing travel request details for employees reporting to the specified manager,
            /// or 500 Internal Server Error response in case of an exception.
            /// </returns>
            [HttpGet("travel/request/ongoing/{managerId}")]
            public async Task<IActionResult> GetManagerOngoingTravelRequest(int managerId)
            {
                try
                {
                    // Call the service method to retrieve ongoing travel request details for employees reporting to the specified manager
                    IEnumerable<ManagerOngoingTravelRequest> ongoingTravelRequestData = await _reportingManagerService.GetManagerOngoingTravelRequestDetails(managerId);

                    // Return a 200 OK response with the retrieved ongoing travel request details
                    return Ok(ongoingTravelRequestData);
                }
                catch (Exception ex)
                {
                    // Handle or log the exception
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting ongoing travel request details: {ex.Message}");
                }
            }

            /// <summary>
            /// Retrieves ongoing travel request details for employees reporting to a specific manager.
            /// </summary>
            /// <param name="managerId">The ID of the reporting manager.</param>
            /// <returns>
            /// 200 OK response with a collection of ongoing travel request details for employees reporting to the specified manager,
            /// or 500 Internal Server Error response in case of an exception.
            /// </returns>
            [HttpGet("travel/request/closed")]
        public async Task<IActionResult> GetTravelRequestClosedAsync(int managerId, int offset = 1, int pageSize = 10)
        {
            try
            {
                // Call the service method to retrieve forwarded travel request details for employees reporting to the specified manager
                var employeeReq = await _reportingManagerService.GetEmployeeRequestsClosedAsync(managerId, offset, pageSize);
                return Ok(employeeReq);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting closed travel request details: {ex.Message}");
            }

        }

        /// <summary>
        /// Get a travel and employee information based on particular request
        /// </summary>
        /// <param name="requestID"></param>
        /// <returns></returns>
        [HttpGet("travel/request/{requestID}")]
        public async Task<IActionResult> GetTravelRequest(int requestID)
        {
            try
            {
                // Call the service method to retrieve ongoing travel request details for employees reporting to the specified manager
                TravelRequestEmployeeViewModel requestData = await _reportingManagerService.GetEmployeeRequestDetail(requestID);
                // Return a 200 OK response with the retrieved ongoing travel request details
                return Ok(requestData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting ongoing travel request details: {ex.Message}");
            }
        }
            

        [HttpPatch("travel/request/approve/")]
        public async Task<IActionResult> TravelPriorityStatusAndRequestApproval([FromBody] UpdatePriorityAndStatusModel updatePriorityAndStatus)
        {
            try
            {
                // Call the service method to retrieve ongoing travel request details for employees reporting to the specified manager
                bool requestData = await _reportingManagerService.UpdateRequestPriorityAndStatus(updatePriorityAndStatus);
                // Return a 200 OK response with the retrieved ongoing travel request details
                return Ok(requestData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting ongoing travel request details: {ex.Message}");
            }
        }

        [HttpPatch("travel/request/cancel/")]
        public async Task<IActionResult> CancelRequest([FromBody] ManagerCancelRequest managerCancelRequest)
        {
            try
            {
                // Call the service method to retrieve ongoing travel request details for employees reporting to the specified manager
                bool requestData = await _reportingManagerService.CancelRequest(managerCancelRequest);
                // Return a 200 OK response with the retrieved ongoing travel request details
                return Ok(requestData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting ongoing travel request details: {ex.Message}");
            }
        }

       //To post to reason table and patch reason id to request table 
        [HttpPost("travel/request/deny")]
        public async Task<IActionResult> PostReasonAndPatchRequest([FromBody] TBL_REASON reason,int reqId)
        {
            try
            {
                await _reportingManagerService.PostReasonForCancellation(reason,reqId);
                return Ok(reason);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding reason for request: {ex.Message}");
            }

        }

        [HttpGet("travel/request/forwarded")]
        public async Task<IActionResult> GetTravelRequestForwardedAsync(int managerId, int offset = 1, int pageSize = 10)
        {
            try
            {
                // Call the service method to retrieve forwarded travel request details for employees reporting to the specified manager
                var employeeReq = await _reportingManagerService.GetEmployeeRequestsForwardedAsync(managerId, offset, pageSize);
                return Ok(employeeReq);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting forwarded travel request details: {ex.Message}");
            }

        }

    }
}


       


       

     