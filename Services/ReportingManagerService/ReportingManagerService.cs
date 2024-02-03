using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XtramileBackend.Data;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;


namespace XtramileBackend.Services.ManagerService
{
    // Service for managing reporting-related functionality
    public class ReportingManagerService : IReportingManagerService
    {
        
        private readonly IUnitOfWork _unitOfWork;


        // Constructor that initializes the service with the database context
        public ReportingManagerService( IUnitOfWork unitOfWork)
        {
            
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get employee requests asynchronously based on managerId
        /// </summary>
        /// <param name="managerId">Manager ID for retrieving the travel request</param>
        /// <returns>List of EmployeeRequestDto</returns>
        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsAsync( int managerId)
        {
            try
            {
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> statusApprovalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> projectMappingData = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();

                var EmpRequest =  (
                  from employee in employeeData
                  join request in requestData on employee.EmpId equals request.CreatedBy
                  join projectMapping in projectMappingData on employee.EmpId equals projectMapping.EmpId
                  join project in projectData on projectMapping.ProjectId equals project.ProjectId
                  join statusApproval in statusApprovalData on request.RequestId equals statusApproval.RequestId
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

        /// <summary>
        /// Get employee requests asynchronously based on managerId and sorted by request code
        /// </summary>
        /// <param name="managerId">Manager ID for retrieving the travel request</param>
        /// <returns>List of EmployeeRequestDto</returns>
        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByRequestCodeAsync( int managerId)
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

        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByEmailAsync(int managerId)
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
                  orderby employee.Email
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

        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByDateAsync( int managerId)
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
        /// Get employee requests for a specific date
        /// </summary>
        /// <param name="managerId"></param>
        /// <param name="date">Date for filtering the requests</param>  
        /// <returns>List of EmployeeRequestDto</returns>

        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsByDateAsync( int managerId, string date)
        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByEmailAsync([FromQuery] int managerId)
        {
            try
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
                }).ToListAsync();

                return EmpRequest;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching the Travel Requests");
                return new List<EmployeeRequestDto>();
            }
        }
        /// <summary>
        /// Get employee requests for a specific email
        /// </summary>
        /// <param name="managerId">Manager ID for getting the requests</param>
        /// <param name="email">Email for filtering the requests</param>
        /// <returns>List of EmployeeRequestDto</returns>
        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsByEmailAsync( int managerId, string email)
        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByDateAsync([FromQuery] int managerId)
        {
            try
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
                  where employee.ReportsTo == managerId && status.StatusCode == "OP" && employee.Email== email
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
                }).ToListAsync();

                return EmpRequest;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching the Travel Requests");
                return new List<EmployeeRequestDto>();
            }
        }

        /// <summary>
        /// to retreives the forwarded requests to a manager
        /// </summary>
        /// <param name="managerId">to retreiev the travel requests to a manager</param>
        /// <returns></returns>
        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsForwardedAsync(int managerId)
        {
            try
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> statusApprovalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> projectMappingData = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();

                var EmpRequest =  (
                from employee in employeeData
                join request in requestData on employee.EmpId equals request.CreatedBy
                join projectMapping in projectMappingData on employee.EmpId equals projectMapping.EmpId
                join project in projectData on projectMapping.ProjectId equals project.ProjectId
                join statusApproval in statusApprovalData on request.RequestId equals statusApproval.RequestId
                join status in statusData on statusApproval.PrimaryStatusId equals status.StatusId
                join status1 in statusData on statusApproval.SecondaryStatusId equals status1.StatusId
                where employee.ReportsTo == managerId && status.StatusCode=="FD" || status1.StatusCode=="FD"
                where TBL_EMPLOYEE.ReportsTo == managerId && TBL_REQUEST.CreatedOn.Date == date.Date
                select new EmployeeRequestDto
                {
                    RequestId = request.RequestId,
                    EmployeeName = employee.FirstName + " " + employee.LastName,
                    Email = employee.Email,
                    ProjectCode = project.ProjectCode,
                    Date = request.CreatedOn,
                    Mode = null,
                    Status = "Forwarded"
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
        /// Get employee requests that are closed asynchronously based on managerId.
        /// </summary>
        /// <param name="managerId">Manager ID for retrieving closed travel requests</param>
        /// <returns>List of closed EmployeeRequestDto</returns>
        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsClosedAsync(int managerId)
        {
            try
            {
                // Query to retrieve closed travel requests


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
                join status1 in statusData on statusApproval.SecondaryStatusId equals status1.StatusId
                where employee.ReportsTo == managerId && status.StatusCode == "CL" || status1.StatusCode == "CL"
                select new EmployeeRequestDto
                {
                    RequestId = request.RequestId,
                    EmployeeName = employee.FirstName + " " + employee.LastName,
                    Email = employee.Email,
                    ProjectCode = project.ProjectCode,
                    Date = request.CreatedOn,
                    Mode = null,
                    Status = "Closed"
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

    }
}