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

        // Get employee requests asynchronously based on managerId
        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsAsync([FromQuery] int managerId)
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

        // Get employee requests sorted by request id
        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByRequestCodeAsync([FromQuery] int managerId)
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

        // Get employee requests sorted by email
        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByEmailAsync([FromQuery] int managerId)
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

        // Get employee requests sorted by date
        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByDateAsync([FromQuery] int managerId)
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

        // Get employee requests for a specific date
        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsByDateAsync([FromQuery] int managerId, DateTime date)
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

        // Get employee requests for a specific email
        public async Task<List<EmployeeRequestDto>> GetEmployeeRequestsByEmailAsync([FromQuery] int managerId, string email)
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
    }
}