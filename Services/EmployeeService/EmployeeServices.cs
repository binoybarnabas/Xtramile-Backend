using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using XtramileBackend.Data;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.StatusService;
using XtramileBackend.UnitOfWork;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace XtramileBackend.Services.EmployeeService
{
    public class EmployeeServices : IEmployeeServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDBContext _dbContext;
        private readonly IStatusServices _statusServices;


        public EmployeeServices(IUnitOfWork unitOfWork, AppDBContext dbContext, IStatusServices statusServices)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _statusServices = statusServices;
        }



        public async Task<IEnumerable<TBL_EMPLOYEE>> GetEmployeeAsync()
        {
            try
            {
                var employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();
                return employeeData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting Employees: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task<EmployeeInfo> GetEmployeeInfo(int id)
        {
            try
            {
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_DEPARTMENT> departmentData = await _unitOfWork.DepartmentRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> projectEmployeeMap = await _unitOfWork.ProjectMappingRepository.GetAllAsync();


                //get the data of the employee
                var employeeDetail = (from projectEmployee in projectEmployeeMap
                                      join project in projectData on projectEmployee.ProjectId equals project.ProjectId
                                      join employee in employeeData.Where(e => e.EmpId == id) on projectEmployee.EmpId equals employee.EmpId
                                      join department in departmentData on project.DepartmentId equals department.DepartmentId
                                      join reportsToEmployee in employeeData on employee.ReportsTo equals reportsToEmployee.EmpId
                                      select new EmployeeInfo
                                      {
                                          FirstName = employee.FirstName,
                                          LastName = employee.LastName,
                                          Email = employee.Email,
                                          ContactNumber = employee.ContactNumber,
                                          Address = employee.Address,
                                          DepartmentName = department.DepartmentName,
                                          ProjectCode = project.ProjectCode,
                                          ProjectName = project.ProjectName,
                                          ReportsTo = reportsToEmployee.FirstName
                                      }).FirstOrDefault();


                return employeeDetail;

            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting Employees: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }



        public async Task SetEmployeeAsync(TBL_EMPLOYEE employee)
        {
            try
            {
                employee.CreatedOn = DateTime.Now;
                await _unitOfWork.EmployeeRepository.AddAsync(employee);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while setting Employee: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task<TBL_EMPLOYEE> GetEmployeeByIdAsync(int id)
        {
            try
            {
                TBL_EMPLOYEE employeeData = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
                return employeeData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while setting Employee: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }

        }

        /// <summary>
        /// Retrieves an employee's profile by their ID.
        /// Perform join operation on the tables TBL_EMPLOYEE, TBL_PROJECT_MAPPING, TBL_PROJECT and TBL_DEPARTMENT.
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <returns>An EmployeeProfile object representing the employee's details.</returns>
        public async Task<EmployeeProfile> GetEmployeeProfileByIdAsync(int employeeId)
        {
            try
            {
                // Fetching data from repositories
                IEnumerable<TBL_EMPLOYEE> employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> projectMappings = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projects = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_DEPARTMENT> departments = await _unitOfWork.DepartmentRepository.GetAllAsync();

                // Querying and joining data to get employee profile
                var result = (
                    from employee in employees
                    join mapping in projectMappings on employee.EmpId equals mapping.EmpId
                    join project in projects on mapping.ProjectId equals project.ProjectId
                    join department in departments on project.DepartmentId equals department.DepartmentId
                    join reportsToEmployee in employees on employee.ReportsTo equals reportsToEmployee.EmpId into reportsToJoin
                    from reportsToEmployee in reportsToJoin.DefaultIfEmpty() // Perform left join inorder to the reports to person full name
                    where employee.EmpId == employeeId
                    select new EmployeeProfile
                    {
                        EmpId = employee.EmpId,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        ContactNumber = employee.ContactNumber,
                        Address = employee.Address,
                        Email = employee.Email,
                        ReportsTo = reportsToEmployee != null ? $"{reportsToEmployee.FirstName} {reportsToEmployee.LastName}" : "N/A",
                        DepartmentName = department.DepartmentName,
                        ProjectCode = project.ProjectCode,
                        ProjectName = project.ProjectName,
                    }
                ).FirstOrDefault();

                // Checking if the result is not null and returning
                if (result != null)
                {
                    return result;
                }
                else
                {
                    // Throwing exception if the employee is not found
                    throw new FileNotFoundException($"Employee with ID {employeeId} not found.");
                }
            }
            catch (Exception ex)
            {
                // Logging and rethrowing the exception
                Console.WriteLine($"An error occurred while getting employee profile by ID: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Updates the details of an employee.
        /// </summary>
        /// <param name="employeeId">The ID of the employee to update.</param>
        /// <param name="profileEdit">A ProfileEdit object containing the updated details.</param>
        public async Task UpdateEmployeeDetailsAsync(int employeeId, ProfileEdit profileEdit)
        {
            // Retrieving the employee from the repository
            TBL_EMPLOYEE employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);

            // Checking if the employee is found
            if (employee != null)
            {
                // Checking if there are updates in the profileEdit object
                if (profileEdit != null)
                {
                    // Updating the contact number if provided
                    if (profileEdit.ContactNumber != null)
                    {
                        employee.ContactNumber = profileEdit.ContactNumber;
                    }

                    // Updating the address if provided
                    if (profileEdit.Address != null)
                    {
                        employee.Address = profileEdit.Address;
                    }

                    // Saving changes to the database
                    await _unitOfWork.SaveChangesAsyn();
                }
            }
            else
            {
                // Throwing exception if the employee is not found
                throw new FileNotFoundException($"Employee with ID {employeeId} not found.");
            }
        }


        /// <summary>
        /// This asynchronous method retrieves a list of OptionCard objects based on a given request ID (reqId).
        /// It fetches data from multiple repositories, performs joins on relevant tables, and projects the results into OptionCard objects.
        /// To handle potential duplicates, the code groups the results by OptionId and selects the first record for each group.
        /// The final result, containing distinct or aggregated OptionCard objects, is returned as an IEnumerable<OptionCard>.
        ///  Exception handling is included to log and propagate any errors that may occur during the process.
        /// </summary>
        /// <param name="reqId"></param>
        /// <returns><IEnumerable<OptionCard></returns>
        public async Task<IEnumerable<OptionCard>> GetOptionsByReqId(int reqId)
        {
            try
            {
                // Fetch data from repositories
                IEnumerable<TBL_AVAIL_OPTION> availableOptions = await _unitOfWork.AvailableOptionRepository.GetAllAsync();
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_TRAVEL_MODE> travelModeData = await _unitOfWork.TravelModeRepository.GetAllAsync();
                IEnumerable<TBL_COUNTRY> countryData = await _unitOfWork.CountryRepository.GetAllAsync();

                // Perform the join and projection
                var result = (from option in availableOptions
                              join request in requestData on option.RequestId equals request.RequestId
                              join mode in travelModeData on option.ModeId equals mode.ModeId
                              join sourceCountry in countryData on request.SourceCountry equals sourceCountry.CountryName
                              join destinationCountry in countryData on request.DestinationCountry equals destinationCountry.CountryName
                              where request.RequestId == reqId
                              select new OptionCard
                              {
                                  OptionId = option.OptionId,
                                  StartTime = option.StartTime,
                                  EndTime = option.EndTime,
                                  ServiceOfferedBy = option.ServiceOfferedBy,
                                  Class = option.Class,
                                  RequestId = option.RequestId,
                                  SourceCity = request.SourceCity,
                                  /*                                  SourceState = request.SourceState,
                                  */
                                  SourceCountry = request.SourceCountry,
                                  SourceCountryCode = sourceCountry.CountryCode,
                                  DestinationCity = request.DestinationCity,
                                  /*                                  DestinationState = request.DestinationState,
                                  */
                                  DestinationCountry = request.DestinationCountry,
                                  DestinationCountryCode = destinationCountry.CountryCode,
                                  ModeId = option.ModeId,
                                  ModeName = mode.ModeName,

                                  TravelType = request.TravelType,

                                  TravelTypeName = request.TravelType


                              })
                              .GroupBy(option => option.OptionId)
                              .Select(group => group.First()) // Keep the first record for each OptionId
                              .ToList();

                return result;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting options for request: {ex.Message}");
                throw;
            }

        }

        /// <summary>
        /// Retrieves pending travel requests for a specific employee.
        /// </summary>
        /// <param name="empId">The employee ID for which to fetch pending requests.</param>
        /// <returns>An asynchronous task returning a collection of PendingRequetsViewEmployee objects.</returns>
        public async Task<IEnumerable<PendingRequetsViewEmployee>> GetPendingRequestsByEmpId(int empId)
        {
            try
            {
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> statusApprovalMap = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_TRAVEL_MODE> travelModeData = await _unitOfWork.TravelModeRepository.GetAllAsync();
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();

                var latestStatusApprovals = statusApprovalMap
                    .GroupBy(approval => approval.RequestId)
                    .Select(group => group.OrderByDescending(approval => approval.date).First());

                var results = (from request in requestData
                               join statusApproval in latestStatusApprovals on request.RequestId equals statusApproval.RequestId
                               join primarystatus in statusData on statusApproval.PrimaryStatusId equals primarystatus.StatusId
                               join secondarystatus in statusData on statusApproval.SecondaryStatusId equals secondarystatus.StatusId
                               join project in projectData on request.ProjectId equals project.ProjectId
                               join travelMode in travelModeData on request.TravelModeId equals travelMode.ModeId
                               join employee in employeeData on statusApproval.EmpId equals employee.EmpId
                               where request.CreatedBy == empId &&
                               ((primarystatus.StatusId != 5 && secondarystatus.StatusId != 5) && //ongoing requests
                                (primarystatus.StatusId != 3 && secondarystatus.StatusId != 3) && //closed requests
                                (primarystatus.StatusId != 9 && secondarystatus.StatusId != 9)) //cancelled requests
                               select new PendingRequetsViewEmployee
                               {
                                   requestId = request.RequestId,
                                   requestCode = request.RequestCode,
                                   projectCode = project.ProjectCode,
                                   tripPurpose = request.TripPurpose,
                                   sourceCity = request.SourceCity,
                                   sourceCountry = request.SourceCountry,
                                   destinationCity = request.DestinationCity,
                                   destinationCountry = request.DestinationCountry,
                                   departureDate = request.DepartureDate,
                                   returnDate = request.ReturnDate,
                                   travelMode = travelMode.ModeName,
                                   statusName = _statusServices.GetStatusName(primarystatus.StatusId, secondarystatus.StatusId),
                                   statusModifiedBy = employee.FirstName + " " + employee.LastName,
                                   date = statusApproval.date
                                   /*                                   destination = request.DestinationCity + ", " +request.DestinationCountry,
                                   *//*                                   dateOfTravel = request.DepartureDate
                                   */
                               })
                               .OrderByDescending(result => result.date) // Add ordering based on the recent status change of a request
                               .ThenByDescending(result => result.requestId) // Add existing ordering by requestId
                               .ToList();
                return results;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting pending requests: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }



        /// <summary>
        /// Retrieves closed requests for an employee
        /// </summary>
        /// <param name="empId"> Employee id for retrieving requests</param>
        /// <returns>An asynchronous task return a collection of type EmpViewRequest</returns>
        public async Task<PagedEmployeeViewReqDto> GeRequestHistoryByEmpId(int empId, int pageIndex, int pageSize)
        {
            try
            {
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> statusApprovalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> projectMappingData = await _unitOfWork.ProjectMappingRepository.GetAllAsync();

                var latestStatusApprovals = statusApprovalData
                .GroupBy(approval => approval.RequestId)
                .Select(group => group.OrderByDescending(approval => approval.date).First());

                var result = (from request in requestData
                              join statusApproval in latestStatusApprovals on request.RequestId equals statusApproval.RequestId
                              join primarystatus in statusData on statusApproval.PrimaryStatusId equals primarystatus.StatusId
                              join secondarystatus in statusData on statusApproval.SecondaryStatusId equals secondarystatus.StatusId
                              join projectMapping in projectMappingData on request.CreatedBy equals projectMapping.EmpId
                              join project in projectData on projectMapping.ProjectId equals project.ProjectId
                              where request.CreatedBy == empId
                               && (primarystatus.StatusCode == "CL" || primarystatus.StatusCode == "CD" || primarystatus.StatusCode == "DD")
                              select new EmployeeViewReq
                              {
                                  RequestId = request.RequestId,
                                  ProjectCode = project.ProjectCode,
                                  ProjectName = project.ProjectName,
                                  TravelType = request.TravelType,
                                  ClosedDate = new DateOnly(statusApproval.date.Year, statusApproval.date.Month, statusApproval.date.Day),
                                  Status = "Closed"

                              }).ToList();

                int totalCount = result.Count();
                int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                var pagedRequest = result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                return new PagedEmployeeViewReqDto
                {
                    EmployeeRequest = pagedRequest,
                    TotatlCount = totalCount,
                    TotalPages = totalPages,
                };
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting closed requests: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }


        /// <summary>
        /// Adds a selected option from the listed available options for a specific request.
        /// </summary>
        /// <param name="option"></param>
        /// <returns>The TBL_REQ_MAPPING object representing the selected option to be added.</returns>
        public async Task AddSelectedOptionForRequest(TBL_REQ_MAPPING option)
        {
            try
            {
                await _unitOfWork.RequestMappingRepository.AddAsync(option);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding options : {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets details of ongoing requests for a specific employee based on the provided employee ID.
        /// Perform join operation on the tables TBL_EMPLOYEE, TBL_PROJECT_MAPPING, TBL_PROJECT,TBL_STATUS, TBL_REQ_APPROVE and TBL_REQUEST.
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <returns>A collection of ongoing request details for the employee.</returns>
        public async Task<IEnumerable<EmployeeOngoingRequest>> GetEmployeeOngoingRequestDetails(int employeeId)
        {
            try
            {
                // Fetching data from repositories
                IEnumerable<TBL_EMPLOYEE> employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projects = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_REQUEST> travelRequests = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> projectMappings = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> reqApprovals = await _unitOfWork.RequestStatusRepository.GetAllAsync();

                // Querying for ongoing request details
                var result = (
                    from reqApproval in reqApprovals
                    join request in travelRequests on reqApproval.RequestId equals request.RequestId
                    join employee in employees on request.CreatedBy equals employee.EmpId
                    join projectMapping in projectMappings on employee.EmpId equals projectMapping.EmpId
                    join project in projects on projectMapping.ProjectId equals project.ProjectId
                    join primaryStatus in statusData on reqApproval.PrimaryStatusId equals primaryStatus.StatusId
                    join secondaryStatus in statusData on reqApproval.SecondaryStatusId equals secondaryStatus.StatusId
                    where request.CreatedBy == employeeId
                        && request.PerdiemId != null
                        && reqApproval.PrimaryStatusId == 5
                        && primaryStatus.StatusCode == "OG"
                        && secondaryStatus.StatusCode == "OG"
                    select new EmployeeOngoingRequest
                    {
                        RequestId = request.RequestId,
                        ProjectCode = project.ProjectCode,
                        ProjectName = project.ProjectName,
                        StartDate = request.DepartureDate,
                        EndDate = request.ReturnDate,
                        Reason = request.TripPurpose,
                        StatusName = primaryStatus.StatusName
                    }
                );

                // Checking if the result is not null and returning
                if (result != null && result.Any()) // Check if there are any results
                {
                    return result.ToList(); // If there are results, return the list
                }
                else
                {
                    // Throwing exception if no employees are found or no matching requests
                    throw new FileNotFoundException($"No employees found with Employee ID {employeeId} with the specified criteria.");
                }
            }
            catch (Exception ex)
            {
                // Logging and rethrowing the exception
                Console.WriteLine($"An error occurred while getting employee details with Employee ID {employeeId}: {ex.Message}");
                throw;
            }
        }
        /// <summary>
        /// Gets details of dashboard upcoming details for a specific employee based on the provided employee ID.
        /// Perform join operation on the tables TBL_EMPLOYEE, TBL_STATUS, TBL_REQ_APPROVE and TBL_REQUEST.
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <returns>A collection of dashboard upcoming details for the employee.</returns>
        public async Task<IEnumerable<DashboardUpcomingTrip>> GetEmployeeDashboardUpcomingTripByIdAsync(int employeeId)
        {
            try
            {
                // Fetching data from repositories
                IEnumerable<TBL_EMPLOYEE> employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                IEnumerable<TBL_REQUEST> travelRequests = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> reqApprovals = await _unitOfWork.RequestStatusRepository.GetAllAsync();

                // Querying for ongoing request details
                var result = (
                    from reqApproval in reqApprovals
                    join request in travelRequests on reqApproval.RequestId equals request.RequestId
                    join primaryStatus in statusData on reqApproval.PrimaryStatusId equals primaryStatus.StatusId
                    where request.CreatedBy == employeeId
                        && reqApproval.PrimaryStatusId == 5
                        && primaryStatus.StatusCode == "OG"
                    select new DashboardUpcomingTrip
                    {
                        StartDate = request.DepartureDate,
                        EndDate = request.ReturnDate,
                        TripPurpose = request.TripPurpose,
                        SourceCity = request.SourceCity,
                        SourceCountry = request.SourceCountry,
                        DestinationCity = request.DestinationCity,
                        DestinationCountry = request.DestinationCountry
                    }
                ).ToList();

                // Checking if the result is not null and returning
                if (result != null && result.Any()) // Check if there are any results
                {
                    return result;// If there are results, return the list
                }
                else
                {
                    return result = null;
                    // Throwing exception if no employees are found or no matching requests
                    throw new FileNotFoundException($"No employees found with Employee ID {employeeId} with the specified criteria.");
                }
            }
            catch (Exception ex)
            {
                // Logging and rethrowing the exception
                Console.WriteLine($"An error occurred while getting employee details for Employee ID {employeeId}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets details of dashboard progress details for a specific employee based on the provided employee ID.
        /// Perform join operation on the tables TBL_EMPLOYEE, TBL_STATUS, TBL_REQ_APPROVE, TBL_ROLES and TBL_REQUEST.
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <returns>A collection of dashboard progress details for the employee.</returns>
        public async Task<IEnumerable<DashboardEmployeeprogress>> GetEmployeeDashboardProgressAsync(int employeeId)
        {
            try
            {
                // Fetching data from repositories
                IEnumerable<TBL_EMPLOYEE> employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                IEnumerable<TBL_REQUEST> travelRequests = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> reqApprovals = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_ROLES> employeeRoles = await _unitOfWork.RoleRepository.GetAllAsync();

                // Querying for dashboard details
                var result = (
                         from reqApproval in reqApprovals
                         join request in travelRequests on reqApproval.RequestId equals request.RequestId
                         join employee in employees on request.CreatedBy equals employee.EmpId
                         join primaryStatus in statusData on reqApproval.PrimaryStatusId equals primaryStatus.StatusId
                         join secondaryStatus in statusData on reqApproval.SecondaryStatusId equals secondaryStatus.StatusId
                         where request.CreatedBy == employeeId 
                         group new { reqApproval, request, primaryStatus, secondaryStatus } by request.RequestId into requestGroup
                         let latestEntry = requestGroup.OrderByDescending(entry => entry.reqApproval.date).FirstOrDefault()
                         let empRole = (
                             from emp in employees
                             join role in employeeRoles on emp.RoleId equals role.RoleId
                             where emp.EmpId == latestEntry?.reqApproval.EmpId
                             select role
                         ).FirstOrDefault()
                         select new DashboardEmployeeprogress
                         {
                             RequestCode = latestEntry.request.RequestCode,
                             SourceCity = latestEntry.request.SourceCity,
                             DestinationCity = latestEntry.request.DestinationCity,
                             Status = latestEntry.primaryStatus.StatusName,
                             Progress = (
                                 (latestEntry.primaryStatus.StatusCode == "OP" && latestEntry.secondaryStatus.StatusCode == "PE") ? "Request Submitted" :
                                 (latestEntry.primaryStatus.StatusCode == "FD" && latestEntry.secondaryStatus.StatusCode == "PE" && empRole?.RoleName == "Manager") ? "Approved by Manager" :
                                 (latestEntry.primaryStatus.StatusCode == "PE" && latestEntry.secondaryStatus.StatusCode == "WT" && empRole?.RoleName == "Travel Admin") ? "Options Sent by Travel Admin" :
                                 (latestEntry.primaryStatus.StatusCode == "PE" && latestEntry.secondaryStatus.StatusCode == "SD" && empRole?.RoleName == "Manager") ? "Options Selected by Manager ":
                                 (latestEntry.primaryStatus.StatusCode == "FD" && latestEntry.secondaryStatus.StatusCode == "FD" && empRole?.RoleName == "Travel Admin") ? "Travel Admin Forwarded" :
                                 (latestEntry.primaryStatus.StatusCode == "OG") ? "Trip Ongoing" :
                                 (latestEntry.primaryStatus.StatusCode == "CL" && latestEntry.secondaryStatus.StatusCode == "CL") ? "Trip Completed" :
                                 (latestEntry.primaryStatus.StatusCode == "CD" && latestEntry.secondaryStatus.StatusCode == "CD") ? "Request Cancelled" :
                                 (latestEntry.primaryStatus.StatusCode == "DD" && latestEntry.secondaryStatus.StatusCode == "PE" && empRole?.RoleName == "Manager") ? "Manager Denied" :
                                 (latestEntry.primaryStatus.StatusCode == "DD" && latestEntry.secondaryStatus.StatusCode == "CD") ? "Resubmitted by Employee" :
                                 "Unknown"
                             )
                         }
                         ).ToList();

                // Checking if the result is not empty and returning
                if (result.Any())
                {
                    return result;
                }
                else
                {
                    // Throwing exception if no matching requests
                    throw new FileNotFoundException($"No dashboard details found for employee ID {employeeId} with the specified criteria.");
                }
            }
            catch (Exception ex)
            {
                // Logging and rethrowing the exception
                Console.WriteLine($"An error occurred while getting dashboard details for employee ID {employeeId}: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<EmployeeCurrentRequest>> getEmployeeCurrentTravel(int empId)
        {
            try
            {
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> statusApprovalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_ROLES> roleData = await _unitOfWork.RoleRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> projectMappingData = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_DEPARTMENT> departmentData = await _unitOfWork.DepartmentRepository.GetAllAsync();

                var currentRequest = (from request in requestData
                                      join
                              employee in employeeData on request.CreatedBy equals employee.EmpId
                                      join
                              role in roleData on employee.RoleId equals role.RoleId
                                      join
                              statusApproval in statusApprovalData on request.RequestId equals statusApproval.RequestId
                                      join
                              status in statusData on statusApproval.PrimaryStatusId equals status.StatusId
                                      join
                              projectMapping in projectMappingData on statusApproval.EmpId equals projectMapping.EmpId
                                      join
                              project in projectData on projectMapping.ProjectId equals project.ProjectId
                                      join
                              department in departmentData on project.DepartmentId equals department.DepartmentId
                                      where status.StatusCode == "OG" || status.StatusCode == "FW" && department.DepartmentName == "Travel"

                                      select new EmployeeCurrentRequest
                                      {
                                          DepartureDate = request.DepartureDate,
                                          ReturnDate = request.ReturnDate,
                                          DepartureTime = request.PrefDepartureTime,
                                          Source = request.SourceState,
                                          Destination = request.DestinationState,
                                          Purpose = request.TripPurpose
                                      }

                ).ToList();
                return currentRequest;

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured");
                throw;
            }


        }

        /// <summary>
        /// Updates the password for a user with the specified email.
        /// </summary>
        /// <param name="email">The email of the user whose password is to be updated.</param>
        /// <param name="newPassword">The new password to set for the user.</param>
        /// <returns>The updated user entity if the password was updated successfully, or null if no user was found.</returns>
        public async Task<TBL_USER> updatePassword(string email, string newPassword)
        {
            try
            {
                var user = await _dbContext.TBL_USER.FirstOrDefaultAsync(u => u.Email == email);
                if (user != null)
                {
                    // Update the user's password
                    user.Password = newPassword;

                    // Save the changes to the database
                    await _dbContext.SaveChangesAsync();
                }

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured");
                throw;
            }
        }

        public async Task<IEnumerable<RequestNotification>> GetEmployeeRequestNotificationsAsync(int empId)
        {
            try
            {
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> reqApprovalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();

                var travelRequest = (
                    from request in requestData
                    join reqApproval in reqApprovalData on request.RequestId equals reqApproval.RequestId
                    join status in statusData on reqApproval.PrimaryStatusId equals status.StatusId
                    where request.CreatedBy == empId
                    orderby reqApproval.date
                    select new RequestNotification
                    {
                        RequestCode = request.RequestCode,
                        StatusName = status.StatusName,
                        Date = reqApproval.date,
                    }

                    ).Take(6).ToList();

                return travelRequest;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching employee request notification {ex.Message}");
                throw;
            }

        }
        /// <summary>
        /// To display the completed trips  of an employee by joining multiple tables
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CompletedTripsCard>> GetCompletedTrips(int empId)
        {
            try
            {
                IEnumerable<TBL_REQUEST> requestsData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> requestApprovalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();

                var latestStatusApprovals = requestApprovalData
                                            .GroupBy(approval => approval.RequestId)
                                            .Select(group => group.OrderByDescending(approval => approval.date).First());

                var completedTrips = (from request in requestsData
                                     join requestApproval in latestStatusApprovals on request.RequestId equals requestApproval.RequestId
                                     where (request.CreatedBy == empId && (requestApproval.PrimaryStatusId == 3 && requestApproval.SecondaryStatusId == 3))
                                     group new { request, requestApproval } by new { request.SourceCity, request.DestinationCity } into groupedRequests
                                     select new CompletedTripsCard 
                                     {
                                         From = groupedRequests.Key.SourceCity,
                                         To = groupedRequests.Key.DestinationCity,
                                         DepartureDate = groupedRequests.First().request.DepartureDate,
                                         ReturnDate = groupedRequests.First().request.ReturnDate,
                                         CompletedDate = groupedRequests.First().requestApproval.date,
                                         Count = groupedRequests.Count()
                                     }).OrderByDescending(completedTrips => completedTrips.CompletedDate);

                return completedTrips;
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine("An error occured : " + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// An employee cancels a request after raising the request and before the request is being approved or rejected by the reporting manager
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="empId"></param>
        /// <returns></returns>
        public async Task<bool> EmployeeCancelRequest(int requestId, int empId)
        {
            try
            {
                TBL_REQUEST existingRequestData = await _unitOfWork.RequestRepository.GetByIdAsync(requestId);

                if (existingRequestData != null)
                {

                    var allStatus = await _unitOfWork.StatusRepository.GetAllAsync();

                    var primaryStatus = allStatus.FirstOrDefault(statusData => statusData.StatusCode == "CL");

                    TBL_REQ_APPROVE approve = new TBL_REQ_APPROVE();

                    approve.RequestId = requestId;

                    approve.EmpId = empId;

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
                Console.WriteLine($"An error occurred while updating the request {requestId}: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Submission of a travel option from the employee among a list of travel request
        /// </summary>
        /// <param name="travelOption"></param>
        /// <returns></returns>
        public async Task SubmitSelectedTravelOptionAsync(TBL_TRAVEL_OPTION_MAPPING travelOption)
        {
            Console.WriteLine(travelOption);
            try
            {
                await _unitOfWork.TravelOptionMappingRepository.AddAsync(travelOption);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured");
                throw;
            }
        }

        /// <summary>
        /// function to get the requests in the pending section which are filtered based on the status
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="primaryStatusCode"></param>
        /// <param name="secondaryStatusCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PendingRequetsViewEmployee>> GetFilteredPendingRequestsByEmpId(int empId, string primaryStatusCode, string secondaryStatusCode)
        {
            try
            {
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> statusApprovalMap = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_TRAVEL_MODE> travelModeData = await _unitOfWork.TravelModeRepository.GetAllAsync();
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();

                var latestStatusApprovals = statusApprovalMap
                    .GroupBy(approval => approval.RequestId)
                    .Select(group => group.OrderByDescending(approval => approval.date).First());

                int primaryStatusId = await _statusServices.GetStatusIdByCode(primaryStatusCode);
                int secondaryStatusId = await _statusServices.GetStatusIdByCode(secondaryStatusCode);

                var results = (from request in requestData
                               join statusApproval in latestStatusApprovals on request.RequestId equals statusApproval.RequestId
                               join primarystatus in statusData on statusApproval.PrimaryStatusId equals primarystatus.StatusId
                               join secondarystatus in statusData on statusApproval.SecondaryStatusId equals secondarystatus.StatusId
                               join project in projectData on request.ProjectId equals project.ProjectId
                               join travelMode in travelModeData on request.TravelModeId equals travelMode.ModeId
                               join employee in employeeData on statusApproval.EmpId equals employee.EmpId
                               where (request.CreatedBy == empId &&
                               (primarystatus.StatusId == primaryStatusId && secondarystatus.StatusId == secondaryStatusId))
                               select new PendingRequetsViewEmployee
                               {
                                   requestId = request.RequestId,
                                   requestCode = request.RequestCode,
                                   projectCode = project.ProjectCode,
                                   tripPurpose = request.TripPurpose,
                                   sourceCity = request.SourceCity,
                                   sourceCountry = request.SourceCountry,
                                   destinationCity = request.DestinationCity,
                                   destinationCountry = request.DestinationCountry,
                                   departureDate = request.DepartureDate,
                                   returnDate = request.ReturnDate,
                                   travelMode = travelMode.ModeName,
                                   statusName = _statusServices.GetStatusName(primarystatus.StatusId, secondarystatus.StatusId),
                                   statusModifiedBy = employee.FirstName + " " + employee.LastName
                                   /*                                   destination = request.DestinationCity + ", " +request.DestinationCountry,
                                   *//*                                   dateOfTravel = request.DepartureDate
                                   */
                               }).OrderByDescending(result => result.requestId).ToList();
                return results;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting pending requests: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}
