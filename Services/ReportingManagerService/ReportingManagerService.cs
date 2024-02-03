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
        /// Get employee requests for a specific email
        /// </summary>
        /// <param name="managerId">Manager ID for getting the requests</param>
        /// <param name="email">Email for filtering the requests</param>
        /// <returns>List of EmployeeRequestDto</returns>
        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsByEmailAsync( int managerId, string email)
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
                join status1 in statusData on statusApproval.SecondaryStatusId equals status1.StatusId
                where employee.ReportsTo == managerId && status.StatusCode=="FD" || status1.StatusCode=="FD"
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
    }
}