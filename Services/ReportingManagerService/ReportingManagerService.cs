using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XtramileBackend.Data;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.ManagerService
{
    // Service for managing reporting-related functionality
    public class ReportingManagerService : IReportingManagerService
    {
        private readonly AppDBContext _context;

        // Constructor that initializes the service with the database context
        public ReportingManagerService(AppDBContext DBContext)
        {
            _context = DBContext;
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
                var EmpRequest = await (
                from TBL_EMPLOYEE in _context.TBL_EMPLOYEE
                join TBL_REQUEST in _context.TBL_REQUEST on TBL_EMPLOYEE.EmpId equals TBL_REQUEST.CreatedBy
                join TBL_PROJECT_MAPPING in _context.TBL_PROJECT_MAPPING on TBL_EMPLOYEE.EmpId equals TBL_PROJECT_MAPPING.EmpId
                join TBL_PROJECT in _context.TBL_PROJECT on TBL_PROJECT_MAPPING.ProjectId equals TBL_PROJECT.ProjectId
                where TBL_EMPLOYEE.ReportsTo == managerId
                select new EmployeeRequestDto
                {
                    RequestId = TBL_REQUEST.RequestId,
                    EmployeeName = TBL_EMPLOYEE.FirstName + " " + TBL_EMPLOYEE.LastName,
                    Email = TBL_EMPLOYEE.Email,
                    ProjectCode = TBL_PROJECT.ProjectCode,
                    Date = TBL_REQUEST.CreatedOn,
                    Mode = null,
                    Status = null
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
        /// Get employee requests asynchronously based on managerId and sorted by request code
        /// </summary>
        /// <param name="managerId">Manager ID for retrieving the travel request</param>
        /// <returns>List of EmployeeRequestDto</returns>
        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByRequestCodeAsync( int managerId)
        {
            try
            {
                var EmpRequest = await (
                from TBL_EMPLOYEE in _context.TBL_EMPLOYEE
                join TBL_REQUEST in _context.TBL_REQUEST on TBL_EMPLOYEE.EmpId equals TBL_REQUEST.CreatedBy
                join TBL_PROJECT_MAPPING in _context.TBL_PROJECT_MAPPING on TBL_EMPLOYEE.EmpId equals TBL_PROJECT_MAPPING.EmpId
                join TBL_PROJECT in _context.TBL_PROJECT on TBL_PROJECT_MAPPING.ProjectId equals TBL_PROJECT.ProjectId
                where TBL_EMPLOYEE.ReportsTo == managerId
                orderby TBL_REQUEST.RequestId
                select new EmployeeRequestDto
                {
                    RequestId = TBL_REQUEST.RequestId,
                    EmployeeName = TBL_EMPLOYEE.FirstName + " " + TBL_EMPLOYEE.LastName,
                    Email = TBL_EMPLOYEE.Email,
                    ProjectCode = TBL_PROJECT.ProjectCode,
                    Date = TBL_REQUEST.CreatedOn,
                    Mode = null,
                    Status = "Open"
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
        /// Get employee requests asynchronously based on managerId and sorted by email
        /// </summary>
        /// <param name="managerId">Manager ID for retrieving the travel request</param>
        /// <returns>List of EmployeeRequestDto</returns>

        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByEmailAsync(int managerId)
        {
            try
            {
                var EmpRequest = await (
                from TBL_EMPLOYEE in _context.TBL_EMPLOYEE
                join TBL_REQUEST in _context.TBL_REQUEST on TBL_EMPLOYEE.EmpId equals TBL_REQUEST.CreatedBy
                join TBL_PROJECT_MAPPING in _context.TBL_PROJECT_MAPPING on TBL_EMPLOYEE.EmpId equals TBL_PROJECT_MAPPING.EmpId
                join TBL_PROJECT in _context.TBL_PROJECT on TBL_PROJECT_MAPPING.ProjectId equals TBL_PROJECT.ProjectId
                where TBL_EMPLOYEE.ReportsTo == managerId
                orderby TBL_EMPLOYEE.Email
                select new EmployeeRequestDto
                {
                    RequestId = TBL_REQUEST.RequestId,
                    EmployeeName = TBL_EMPLOYEE.FirstName + " " + TBL_EMPLOYEE.LastName,
                    Email = TBL_EMPLOYEE.Email,
                    ProjectCode = TBL_PROJECT.ProjectCode,
                    Date = TBL_REQUEST.CreatedOn,
                    Mode = null,
                    Status = null
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
        /// Get employee requests asynchronously based on managerId and sorted by date
        /// </summary>
        /// <param name="managerId">Manager ID for retrieving the travel request</param>
        /// <returns>List of EmployeeRequestDto</returns>

        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByDateAsync( int managerId)
        {
            try
            {
                var EmpRequest = await (
                from TBL_EMPLOYEE in _context.TBL_EMPLOYEE
                join TBL_REQUEST in _context.TBL_REQUEST on TBL_EMPLOYEE.EmpId equals TBL_REQUEST.CreatedBy
                join TBL_PROJECT_MAPPING in _context.TBL_PROJECT_MAPPING on TBL_EMPLOYEE.EmpId equals TBL_PROJECT_MAPPING.EmpId
                join TBL_PROJECT in _context.TBL_PROJECT on TBL_PROJECT_MAPPING.ProjectId equals TBL_PROJECT.ProjectId
                where TBL_EMPLOYEE.ReportsTo == managerId
                orderby TBL_REQUEST.CreatedOn
                select new EmployeeRequestDto
                {
                    RequestId = TBL_REQUEST.RequestId,
                    EmployeeName = TBL_EMPLOYEE.FirstName + " " + TBL_EMPLOYEE.LastName,
                    Email = TBL_EMPLOYEE.Email,
                    ProjectCode = TBL_PROJECT.ProjectCode,
                    Date = TBL_REQUEST.CreatedOn,
                    Mode = null,
                    Status = null
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
        /// Get employee requests for a specific date
        /// </summary>
        /// <param name="managerId"></param>
        /// <param name="date">Date for filtering the requests</param>  
        /// <returns>List of EmployeeRequestDto</returns>

        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsByDateAsync( int managerId, DateTime date)
        {
            try
            {
                var EmpRequest = await (
                from TBL_EMPLOYEE in _context.TBL_EMPLOYEE
                join TBL_REQUEST in _context.TBL_REQUEST on TBL_EMPLOYEE.EmpId equals TBL_REQUEST.CreatedBy
                join TBL_PROJECT_MAPPING in _context.TBL_PROJECT_MAPPING on TBL_EMPLOYEE.EmpId equals TBL_PROJECT_MAPPING.EmpId
                join TBL_PROJECT in _context.TBL_PROJECT on TBL_PROJECT_MAPPING.ProjectId equals TBL_PROJECT.ProjectId
                where TBL_EMPLOYEE.ReportsTo == managerId && TBL_REQUEST.CreatedOn.Date == date.Date
                select new EmployeeRequestDto
                {
                    RequestId = TBL_REQUEST.RequestId,
                    EmployeeName = TBL_EMPLOYEE.FirstName + " " + TBL_EMPLOYEE.LastName,
                    Email = TBL_EMPLOYEE.Email,
                    ProjectCode = TBL_PROJECT.ProjectCode,
                    Date = TBL_REQUEST.CreatedOn,
                    Mode = null,
                    Status = "Open"
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
        {
            try
            {
                var EmpRequest = await (
                from TBL_EMPLOYEE in _context.TBL_EMPLOYEE
                join TBL_REQUEST in _context.TBL_REQUEST on TBL_EMPLOYEE.EmpId equals TBL_REQUEST.CreatedBy
                join TBL_PROJECT_MAPPING in _context.TBL_PROJECT_MAPPING on TBL_EMPLOYEE.EmpId equals TBL_PROJECT_MAPPING.EmpId
                join TBL_PROJECT in _context.TBL_PROJECT on TBL_PROJECT_MAPPING.ProjectId equals TBL_PROJECT.ProjectId
                where TBL_EMPLOYEE.ReportsTo == managerId && TBL_EMPLOYEE.Email == email
                select new EmployeeRequestDto
                {
                    RequestId = TBL_REQUEST.RequestId,
                    EmployeeName = TBL_EMPLOYEE.FirstName + " " + TBL_EMPLOYEE.LastName,
                    Email = TBL_EMPLOYEE.Email,
                    ProjectCode = TBL_PROJECT.ProjectCode,
                    Date = TBL_REQUEST.CreatedOn,
                    Mode = null,
                    Status = null
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
            {
                var EmpRequest = await (
                from TBL_EMPLOYEE in _context.TBL_EMPLOYEE
                join TBL_REQUEST in _context.TBL_REQUEST on TBL_EMPLOYEE.EmpId equals TBL_REQUEST.CreatedBy
                join TBL_PROJECT_MAPPING in _context.TBL_PROJECT_MAPPING on TBL_EMPLOYEE.EmpId equals TBL_PROJECT_MAPPING.EmpId
                join TBL_PROJECT in _context.TBL_PROJECT on TBL_PROJECT_MAPPING.ProjectId equals TBL_PROJECT.ProjectId
                join TBL_REQ_APPROVE in _context.TBL_REQ_APPROVE on TBL_REQUEST.RequestId equals TBL_REQ_APPROVE.RequestId
                join TBL_STATUS in _context.TBL_STATUS on TBL_REQ_APPROVE.PrimaryStatusId equals TBL_STATUS.StatusId
                join tbl_Status in _context.TBL_STATUS on TBL_REQ_APPROVE.SecondaryStatusId equals tbl_Status.StatusId
                where TBL_EMPLOYEE.ReportsTo == managerId && TBL_STATUS.StatusCode=="FD" || tbl_Status.StatusCode=="FD"
                select new EmployeeRequestDto
                {
                    RequestId = TBL_REQUEST.RequestId,
                    EmployeeName = TBL_EMPLOYEE.FirstName + " " + TBL_EMPLOYEE.LastName,
                    Email = TBL_EMPLOYEE.Email,
                    ProjectCode = TBL_PROJECT.ProjectCode,
                    Date = TBL_REQUEST.CreatedOn,
                    Mode = null,
                    Status = "Forwarded"
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
        /// Get employee requests that are closed asynchronously based on managerId.
        /// </summary>
        /// <param name="managerId">Manager ID for retrieving closed travel requests</param>
        /// <returns>List of closed EmployeeRequestDto</returns>
        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsClosedAsync(int managerId)
        {
            try
            {
                // Query to retrieve closed travel requests

                var EmpRequest = await (
                from TBL_EMPLOYEE in _context.TBL_EMPLOYEE
                join TBL_REQUEST in _context.TBL_REQUEST on TBL_EMPLOYEE.EmpId equals TBL_REQUEST.CreatedBy
                join TBL_PROJECT_MAPPING in _context.TBL_PROJECT_MAPPING on TBL_EMPLOYEE.EmpId equals TBL_PROJECT_MAPPING.EmpId
                join TBL_PROJECT in _context.TBL_PROJECT on TBL_PROJECT_MAPPING.ProjectId equals TBL_PROJECT.ProjectId
                join TBL_REQ_APPROVE in _context.TBL_REQ_APPROVE on TBL_REQUEST.RequestId equals TBL_REQ_APPROVE.RequestId
                join TBL_STATUS in _context.TBL_STATUS on TBL_REQ_APPROVE.PrimaryStatusId equals TBL_STATUS.StatusId
                join tbl_Status in _context.TBL_STATUS on TBL_REQ_APPROVE.SecondaryStatusId equals tbl_Status.StatusId
                where TBL_EMPLOYEE.ReportsTo == managerId && TBL_STATUS.StatusCode == " CL" || tbl_Status.StatusCode == "CL"
                select new EmployeeRequestDto
                {
                    RequestId = TBL_REQUEST.RequestId,
                    EmployeeName = TBL_EMPLOYEE.FirstName + " " + TBL_EMPLOYEE.LastName,
                    Email = TBL_EMPLOYEE.Email,
                    ProjectCode = TBL_PROJECT.ProjectCode,
                    Date = TBL_REQUEST.CreatedOn,
                    Mode = null,
                    Status = "Closed"
                }).ToListAsync();

                return EmpRequest;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching the Travel Requests");
                return new List<EmployeeRequestDto>();
            }
        }
    }
}