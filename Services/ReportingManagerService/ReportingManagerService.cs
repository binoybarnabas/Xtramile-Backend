using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using XtramileBackend.Data;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace XtramileBackend.Services.ManagerService
{
    // Service for managing reporting-related functionality
    public class ReportingManagerService : IReportingManagerService
    {

        private readonly IUnitOfWork _unitOfWork;


        // Constructor that initializes the service with the database context
        public ReportingManagerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get employee requests asynchronously based on managerId
        /// </summary>
        /// <param name="managerId">Manager ID for retrieving the travel request</param>
        /// <returns>List of EmployeeRequestDto</returns>
        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsAsync(int managerId)
        {
            try
            {
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> statusApprovalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> projectMappingData = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();

                var latestStatusApprovals = statusApprovalData
                    .GroupBy(approval => approval.RequestId)
                    .Select(group => group.OrderByDescending(approval => approval.date).First());

                var EmpRequest = (
                  from employee in employeeData
                  join request in requestData on employee.EmpId equals request.CreatedBy
                  join projectMapping in projectMappingData on employee.EmpId equals projectMapping.EmpId
                  join project in projectData on projectMapping.ProjectId equals project.ProjectId
                  join statusApproval in latestStatusApprovals on request.RequestId equals statusApproval.RequestId
                  join status in statusData on statusApproval.PrimaryStatusId equals status.StatusId
                  where employee.ReportsTo == managerId && status.StatusCode == "OP"
                  select new EmployeeRequestDto
                  {
                      RequestId = request.RequestId,
                      EmployeeName = employee.FirstName + " " + employee.LastName,
                      Email = employee.Email,
                      ProjectCode = project.ProjectCode,
                      Date = request.CreatedOn,
                      Mode = null,
                      Status = status.StatusName
                  }).ToList();

                return EmpRequest;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching the Travel Requests");
                return new List<EmployeeRequestDto>();
            }
        }

        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsByDateAsync(int managerId, string date)
        {
            try
            {
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> statusApprovalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> projectMappingData = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();

                var EmpRequest = (
                  from employee in employeeData
                  join request in requestData on employee.EmpId equals request.CreatedBy
                  join projectMapping in projectMappingData on employee.EmpId equals projectMapping.EmpId
                  join project in projectData on projectMapping.ProjectId equals project.ProjectId
                  join statusApproval in statusApprovalData on request.RequestId equals statusApproval.RequestId
                  join status in statusData on statusApproval.PrimaryStatusId equals status.StatusId
                  where employee.ReportsTo == managerId && status.StatusCode == "OP" && request.CreatedOn.Date == DateTime.ParseExact(date, "yyyy-MM-dd", null)

                  select new EmployeeRequestDto
                  {
                      RequestId = request.RequestId,
                      EmployeeName = employee.FirstName + " " + employee.LastName,
                      Email = employee.Email,
                      ProjectCode = project.ProjectCode,
                      Date = request.CreatedOn,
                      Mode = null,
                      Status = status.StatusName
                  }).ToList();

                return EmpRequest;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching the Travel Requests");
                return new List<EmployeeRequestDto>();
            }

        }

        /// <summary>
        /// Get employee requests asynchronously based on managerId and sorted by request code
        /// </summary>
        /// <param name="managerId">Manager ID for retrieving the travel request</param>
        /// <returns>List of EmployeeRequestDto</returns>
        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByRequestCodeAsync(int managerId)
        {
            try
            {
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> statusApprovalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> projectMappingData = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();

                var EmpRequest = (
                  from employee in employeeData
                  join request in requestData on employee.EmpId equals request.CreatedBy
                  join projectMapping in projectMappingData on employee.EmpId equals projectMapping.EmpId
                  join project in projectData on projectMapping.ProjectId equals project.ProjectId
                  join statusApproval in statusApprovalData on request.RequestId equals statusApproval.RequestId
                  join status in statusData on statusApproval.PrimaryStatusId equals status.StatusId
                  where employee.ReportsTo == managerId && status.StatusCode == "OP"
                  orderby request.RequestId
                  select new EmployeeRequestDto
                  {
                      RequestId = request.RequestId,
                      EmployeeName = employee.FirstName + " " + employee.LastName,
                      Email = employee.Email,
                      ProjectCode = project.ProjectCode,
                      Date = request.CreatedOn,
                      Mode = null,
                      Status = status.StatusName
                  }).ToList();

                return EmpRequest;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching the Travel Requests");
                return new List<EmployeeRequestDto>();
            }
        }

        /// <summary>
        /// Get employee requests asynchronously based on managerId and sorted by email
        /// </summary>
        /// <param name="managerId">Manager ID for retrieving the travel request</param>
        /// <returns>List of EmployeeRequestDto</returns>
        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByEmployeeNameAsync(int managerId)
        {
            try
            {
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> statusApprovalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> projectMappingData = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();

                var EmpRequest = (
                  from employee in employeeData
                  join request in requestData on employee.EmpId equals request.CreatedBy
                  join projectMapping in projectMappingData on employee.EmpId equals projectMapping.EmpId
                  join project in projectData on projectMapping.ProjectId equals project.ProjectId
                  join statusApproval in statusApprovalData on request.RequestId equals statusApproval.RequestId
                  join status in statusData on statusApproval.PrimaryStatusId equals status.StatusId
                  where employee.ReportsTo == managerId && status.StatusCode == "OP"
                  orderby employee.FirstName, employee.LastName
                  select new EmployeeRequestDto
                  {
                      RequestId = request.RequestId,
                      EmployeeName = employee.FirstName + " " + employee.LastName,
                      Email = employee.Email,
                      ProjectCode = project.ProjectCode,
                      Date = request.CreatedOn,
                      Mode = null,
                      Status = status.StatusName
                  }).ToList();

                return EmpRequest;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching the Travel Requests");
                return new List<EmployeeRequestDto>();
            }
        }

        /// <summary>
        /// Get employee requests asynchronously based on managerId and sorted by date
        /// </summary>
        /// <param name="managerId">Manager ID for retrieving the travel request</param>
        /// <returns>List of EmployeeRequestDto</returns>
        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByDateAsync(int managerId)
        {
            try
            {
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> statusApprovalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> projectMappingData = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();

                var EmpRequest = (
                  from employee in employeeData
                  join request in requestData on employee.EmpId equals request.CreatedBy
                  join projectMapping in projectMappingData on employee.EmpId equals projectMapping.EmpId
                  join project in projectData on projectMapping.ProjectId equals project.ProjectId
                  join statusApproval in statusApprovalData on request.RequestId equals statusApproval.RequestId
                  join status in statusData on statusApproval.PrimaryStatusId equals status.StatusId
                  where employee.ReportsTo == managerId && status.StatusCode == "OP"
                  orderby request.CreatedOn

                  select new EmployeeRequestDto
                  {
                      RequestId = request.RequestId,
                      EmployeeName = employee.FirstName + " " + employee.LastName,
                      Email = employee.Email,
                      ProjectCode = project.ProjectCode,
                      Date = request.CreatedOn,
                      Mode = null,
                      Status = status.StatusName
                  }).ToList();

                return EmpRequest;

                return EmpRequest;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching the Travel Requests");
                return new List<EmployeeRequestDto>();
            }
        }

        /// <summary>
       /// to retreives the forwarded travel requests for a manager
        /// </summary>
        /// <param name="managerId">to retrieve the travel requests for a particular manager</param>
        /// <param name="offset">to set the  page index of the table</param>
        /// <param name="pageSize">to set the number of rows of paginated table </param>
        /// <returns></returns>
        public async Task<PagedEmployeeRequestDto> GetEmployeeRequestsForwardedAsync(int managerId, int offset, int pageSize)
        {
            try
            {
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> statusApprovalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> projectMappingData = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();

                var latestStatusApprovals = statusApprovalData
                    .GroupBy(approval => approval.RequestId)
                    .Select(group => group.OrderByDescending(approval => approval.date).First());

                var EmpRequest = (
                    from employee in employeeData
                    join request in requestData on employee.EmpId equals request.CreatedBy
                    join projectMapping in projectMappingData on employee.EmpId equals projectMapping.EmpId
                    join project in projectData on projectMapping.ProjectId equals project.ProjectId
                    join statusApproval in latestStatusApprovals on request.RequestId equals statusApproval.RequestId
                    join status in statusData on statusApproval.PrimaryStatusId equals status.StatusId
                    join status1 in statusData on statusApproval.SecondaryStatusId equals status1.StatusId
                    where employee.ReportsTo == managerId
                    && status.StatusCode == "FD" || status1.StatusCode == "FD"
                    select new EmployeeRequestDto
                    {
                        RequestId = request.RequestId,
                        EmployeeName = employee.FirstName + " " + employee.LastName,
                        Email = employee.Email,
                        ProjectCode = project.ProjectCode,
                        Date = request.CreatedOn,
                        Mode = null,
                        Status = status.StatusName  
                    }).ToList();

                var totalCount = EmpRequest.Count();    
                var totalPages= (int)Math.Ceiling((double   )totalCount / pageSize);
                var pagedEmployeeRequests= EmpRequest.Skip((offset - 1) * pageSize).Take(pageSize).ToList();
                return new PagedEmployeeRequestDto
                {
                    EmployeeRequest = pagedEmployeeRequests,
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching the Travel Requests: " + ex.Message);
                return new PagedEmployeeRequestDto();
            }
        }


        /// <summary>
        ///  Get employee requests that are closed asynchronously based on managerId.
        /// </summary>
        /// <param name="managerId"> to retrieve the travel requests</param>
        /// <param name="offset">to set the page index for paginated table</param>
        /// <param name="pageSize">to set the number of rows in paginated table</param>
        /// <returns>an object of PagedEmployeeRequestDto </returns>
        public async Task<PagedEmployeeRequestDto> GetEmployeeRequestsClosedAsync(int managerId, int offset, int pageSize)
        {
            try
            {
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> statusApprovalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> projectMappingData = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();

                var latestStatusApprovals = statusApprovalData
                    .GroupBy(approval => approval.RequestId)
                    .Select(group => group.OrderByDescending(approval => approval.date).First());

                var EmpRequest = (
                    from employee in employeeData
                    join request in requestData on employee.EmpId equals request.CreatedBy
                    join projectMapping in projectMappingData on employee.EmpId equals projectMapping.EmpId
                    join project in projectData on projectMapping.ProjectId equals project.ProjectId
                    join statusApproval in latestStatusApprovals on request.RequestId equals statusApproval.RequestId
                    join status in statusData on statusApproval.PrimaryStatusId equals status.StatusId
                    join status1 in statusData on statusApproval.SecondaryStatusId equals status1.StatusId
                    where employee.ReportsTo == managerId
                    && status.StatusCode == "CL"
                    select new EmployeeRequestDto
                    {
                        RequestId = request.RequestId,
                        EmployeeName = employee.FirstName + " " + employee.LastName,
                        Email = employee.Email,
                        ProjectCode = project.ProjectCode,
                        Date = request.CreatedOn,
                        Mode = null,
                        Status = status.StatusName
                    }).ToList();

                var totalCount = EmpRequest.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                var pagedEmployeeRequests = EmpRequest.Skip((offset - 1) * pageSize).Take(pageSize).ToList();
                return new PagedEmployeeRequestDto
                {
                    EmployeeRequest = pagedEmployeeRequests,
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching the Closed Travel Requests: " + ex.Message);
                return new PagedEmployeeRequestDto();
            }
        }


        /// <summary>

        /// Get all the requests for a particular employee based on the name of the employee
        /// </summary>
        /// <param name="managerId"></param>
        /// <param name="employeeName"></param>
        /// <returns>
        /// A list of Request data of a particular employee which contains information like Request Id, Employee name, Email, project code, date and status
        /// </returns>
        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsByEmployeeNameAsync(int managerId, string employeeName)
        {
            try
            {
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> statusApprovalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> projectMappingData = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();

                var EmpRequest = (
                  from employee in employeeData
                  join request in requestData on employee.EmpId equals request.CreatedBy
                  join projectMapping in projectMappingData on employee.EmpId equals projectMapping.EmpId
                  join project in projectData on projectMapping.ProjectId equals project.ProjectId
                  join statusApproval in statusApprovalData on request.RequestId equals statusApproval.RequestId
                  join status in statusData on statusApproval.PrimaryStatusId equals status.StatusId
                  where employee.ReportsTo == managerId && status.StatusCode == "OP"
                   && (employee.FirstName + " " + employee.LastName).Contains(employeeName)
                  select new EmployeeRequestDto
                  {
                      RequestId = request.RequestId,
                      EmployeeName = employee.FirstName + " " + employee.LastName,
                      Email = employee.Email,
                      ProjectCode = project.ProjectCode,
                      Date = request.CreatedOn,
                      Mode = null,
                      Status = status.StatusName
                  }).ToList();
                return EmpRequest;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching the Employee requests Requests");
                return new List<EmployeeRequestDto>();
            }
        }

        /// Retrieves ongoing travel request details for employees reporting to a specific manager.
        /// </summary>
        /// <param name="managerId">The ID of the reporting manager.</param>
        /// <returns>
        /// A collection of ongoing travel request details for employees reporting to the specified manager,
        /// including information such as Request ID, Employee Name, Employee Email, Project Code, Created On,
        /// Travel Type Name, Priority Name, and Status Name.
        /// </returns>
        public async Task<IEnumerable<ManagerOngoingTravelRequest>> GetManagerOngoingTravelRequestDetails(int managerId)
        {
            try
            {
                // Fetching data from repositories
                IEnumerable<TBL_EMPLOYEE> employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                IEnumerable<TBL_REQ_MAPPING> reqMappings = await _unitOfWork.RequestMappingRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projects = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_REQUEST> travelRequests = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> projectMappings = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                IEnumerable<TBL_DEPARTMENT> departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
                IEnumerable<TBL_TRAVEL_TYPE> travelTypes = await _unitOfWork.TravelTypeRepository.GetAllAsync();
                IEnumerable<TBL_PRIORITY> priorities = await _unitOfWork.PriorityRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> reqApprovals = await _unitOfWork.RequestStatusRepository.GetAllAsync();

                // LINQ query to retrieve ongoing travel request details
                var result = (
                    from request in travelRequests
                    join mapping in reqMappings on request.RequestId equals mapping.RequestId
                    join employee in employees on mapping.EmpId equals employee.EmpId
                    join projectMapping in projectMappings on employee.EmpId equals projectMapping.EmpId
                    join project in projects on projectMapping.ProjectId equals project.ProjectId
                    join department in departments on project.DepartmentId equals department.DepartmentId
                    join travelType in travelTypes on request.TravelTypeId equals travelType.TravelTypeID
                    join priority in priorities on request.PriorityId equals priority.PriorityId
                    join reqApproval in reqApprovals on request.RequestId equals reqApproval.RequestId
                    join primaryStatus in statusData on reqApproval.PrimaryStatusId equals primaryStatus.StatusId
                    join secondaryStatus in statusData on reqApproval.SecondaryStatusId equals secondaryStatus.StatusId
                    where employee.ReportsTo == managerId
                        && request.PerdiemId != null
                        && reqApproval.PrimaryStatusId == 5
                        && primaryStatus.StatusCode == "OG"
                        && secondaryStatus.StatusCode == "OG"
                    select new ManagerOngoingTravelRequest
                    {
                        RequestId = request.RequestId,
                        EmployeeName = $"{employee.FirstName} {employee.LastName}",
                        EmployeeEmail = employee.Email,
                        ProjectCode = project.ProjectCode,
                        CreatedOn = request.CreatedOn,
                        TravelTypeName = travelType.TypeName,
                        PriorityName = priority.PriorityName,
                        StatusName = primaryStatus.StatusName
                    }
                );

                // Checking if the result is not null and returning
                if (result != null && result.Any())
                {
                    return result.ToList();
                }
                else
                {
                    // Throwing exception if no employees are found or no matching requests
                    throw new FileNotFoundException($"No employees found who report to Manager ID {managerId} with the specified criteria.");
                }
            }
            catch (Exception ex)
            {
                // Logging and rethrowing the exception
                Console.WriteLine($"An error occurred while getting employee details for Manager ID {managerId}: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Get the travel information and employee information from a particular request raised by an employee
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns>
        /// a list that contains
        /// RequestId, FirstName, LastName, ContactNumber, Email, ReportsTo, DepartmentName, ProjectCode,
        /// ProjectName, TravelType, TripPurpose, DepartureDate, ReturnDate, SourceCityZipCode, DestinationCityZipCode,
        /// SourceCity, DestinationCity, SourceState, DestinationState, SourceCountry, DestinationCountry, CabRequired,
        /// AccommodationRequired, PrefDepartureTime, 
        /// TravelAuthorizationEmailCapture, PassportAttachment, IdCardAttachment, AdditionalComments
        /// </returns>
        public async Task<TravelRequestEmployeeViewModel> GetEmployeeRequestDetail(int requestId)
        {
            try
            {

                IEnumerable<TBL_EMPLOYEE> employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> reqApprovals = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_REQUEST> travelRequests = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_TRAVEL_TYPE> travelTypes = await _unitOfWork.TravelTypeRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> projectMappings = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projects = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_DEPARTMENT> departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
                var employeeRequestDetail = (from employee in employees
                                             join travelRequest in travelRequests on employee.EmpId equals travelRequest.CreatedBy
                                             join travelType in travelTypes on travelRequest.TravelTypeId equals travelType.TravelTypeID
                                             join projectMapping in projectMappings on employee.EmpId equals projectMapping.EmpId
                                             join project in projects on projectMapping.ProjectId equals project.ProjectId
                                             join department in departments on project.DepartmentId equals department.DepartmentId
                                             join reportsToEmployee in employees on employee.ReportsTo equals reportsToEmployee.EmpId
                                             where travelRequest.RequestId == requestId
                                             select new TravelRequestEmployeeViewModel
                                             {
                                                 RequestId = travelRequest.RequestId,
                                                 FirstName = employee.FirstName,
                                                 LastName = employee.LastName,
                                                 ContactNumber = employee.ContactNumber,
                                                 Email = employee.Email,
                                                 ReportsTo = reportsToEmployee.FirstName + " " + reportsToEmployee.LastName,
                                                 DepartmentName = department.DepartmentName,
                                                 ProjectCode = project.ProjectCode,
                                                 ProjectName = project.ProjectName,
                                                 TravelType = travelType.TypeName,
                                                 TripPurpose = travelRequest.TripPurpose,
                                                 DepartureDate = travelRequest.DepartureDate,
                                                 ReturnDate = travelRequest.ReturnDate,
                                                 SourceCityZipCode = travelRequest.SourceCityZipCode,
                                                 DestinationCityZipCode = travelRequest.DestinationCityZipCode,
                                                 SourceCity = travelRequest.SourceCity,
                                                 DestinationCity = travelRequest.DestinationCity,
                                                 SourceState = travelRequest.SourceState,
                                                 DestinationState = travelRequest.DestinationState,
                                                 SourceCountry = travelRequest.SourceCountry,
                                                 DestinationCountry = travelRequest.DestinationCountry,
                                                 CabRequired = travelRequest.CabRequired,
                                                 AccommodationRequired = travelRequest.AccommodationRequired,
                                                 PrefDepartureTime = travelRequest.PrefDepartureTime,
                                                 CreatedBy = travelRequest.CreatedBy,
                                                 /*TravelAuthorizationEmailCapture =
                                                 PassportAttachment =
                                                 IdCardAttachment = */
                                                 AdditionalComments = travelRequest.AdditionalComments
                                             }
                                             );

                return employeeRequestDetail.FirstOrDefault();

            }
            catch (Exception ex)
            {
                // Logging and rethrowing the exception
                Console.WriteLine($"An error occurred while getting employee request details for request {requestId}: {ex.Message}");
                throw;
            }
        }



        /// <summary>
        /// /// Update the request status and priority for a request that comes under a manager
        /// </summary>
        /// <param name="updatePriorityAndStatus"></param>
        /// <returns> return true if the update is successful</returns>
        public async Task<bool> UpdateRequestPriorityAndStatus(UpdatePriorityAndStatusModel updatePriorityAndStatus)
        {
            try
            {

                TBL_REQUEST existingRequest = await _unitOfWork.RequestRepository.GetByIdAsync(updatePriorityAndStatus.RequestId);

                var allStatus = await _unitOfWork.StatusRepository.GetAllAsync();

                var previousPrimaryStatus = allStatus.FirstOrDefault(statusData => statusData.StatusCode == "OP");

                var primaryStatus = allStatus.FirstOrDefault(statusData => statusData.StatusCode == "FD");

                var secondaryStatus = allStatus.FirstOrDefault(statusData => statusData.StatusCode == "PE");

                if (existingRequest != null)
                {
                    existingRequest.PriorityId = updatePriorityAndStatus.PriorityId;
                }

                TBL_REQ_APPROVE approve = new TBL_REQ_APPROVE();

                approve.RequestId = updatePriorityAndStatus.RequestId;

                approve.EmpId = updatePriorityAndStatus.ManagerId;

                approve.PrimaryStatusId = primaryStatus.StatusId;

                approve.SecondaryStatusId = secondaryStatus.StatusId;

                approve.date = DateTime.Now;

                await _unitOfWork.RequestStatusRepository.AddAsync(approve);

                existingRequest.PriorityId = updatePriorityAndStatus.PriorityId;

                //once the status is updated the new priority and status needs to be set is request status mapping
                // and delete the older priority and status. here it is OP for open requests
                _unitOfWork.RequestRepository.Update(existingRequest);

                _unitOfWork.Complete();

                return true;
            }
            catch (Exception ex)
            {
                // Logging and rethrowing the exception
                Console.WriteLine($"An error occurred while updating the request {updatePriorityAndStatus.RequestId}: {ex.Message}");
                throw;
            }

        }

        /// <summary>
        /// Cancellation of request by a manager based on a particular request that comes under the status open by the employee
        /// </summary>
        /// <param name="managerCancelRequest"></param>
        /// <returns>
        /// returns true if the request is cancelled.
        /// </returns>
        public async Task<bool> CancelRequest(ManagerCancelRequest managerCancelRequest)
        {
            try
            {
                TBL_REQUEST existingRequestData = await _unitOfWork.RequestRepository.GetByIdAsync(managerCancelRequest.RequestId);

                if (existingRequestData != null) {

                    var allStatus = await _unitOfWork.StatusRepository.GetAllAsync();

                    var previousPrimaryStatus = allStatus.FirstOrDefault(statusData => statusData.StatusCode == "OP");

                    var primaryStatus = allStatus.FirstOrDefault(statusData => statusData.StatusCode == "CL");

                    TBL_REQ_APPROVE approve = new TBL_REQ_APPROVE();

                    approve.RequestId = managerCancelRequest.RequestId;

                    approve.EmpId = managerCancelRequest.ManagerId;

                    approve.PrimaryStatusId = primaryStatus.StatusId;

                    approve.SecondaryStatusId = primaryStatus.StatusId;

                    approve.date = DateTime.Now;

                    await _unitOfWork.RequestStatusRepository.AddAsync(approve);

                    _unitOfWork.Complete();
                }
                return true;

            }
            catch (Exception ex)
            {
                // Logging and rethrowing the exception
                Console.WriteLine($"An error occurred while updating the request {managerCancelRequest.RequestId}: {ex.Message}");
                throw;
            }
        }
        /// <summary>
        /// To post the reason data to TBL_REASON and patch the reasonid to \
        /// corresponding request using requestid.
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="reqId"></param>
        /// <returns></returns>
        public async Task PostReasonForCancellation(TBL_REASON reason, int reqId)
        {
            try
            {
                reason.ReasonCode = GenerateReasonCode(reqId);
                await _unitOfWork.ReasonRepository.AddAsync(reason);
                _unitOfWork.Complete();
                var request = await _unitOfWork.RequestRepository.GetByIdAsync(reqId);//to get the corresponding request
                request.ReasonId = reason.ReasonId;
                _unitOfWork.RequestRepository.Update(request);
                _unitOfWork.Complete();
            }
            catch (Exception ex) {
                Console.WriteLine($"An error occurred while adding reason to the request {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// to generate a random reasoncode using reqid
        /// </summary>
        /// <param name="reqId"></param>
        /// <returns></returns>
        private string GenerateReasonCode(int reqId)
        {
            // For example, you can concatenate "RE" with the ReasonId
            return "RE" + reqId.ToString("D6"); // Example: RE000001
        }

    }
}