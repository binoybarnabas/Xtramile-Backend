﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XtramileBackend.Data;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.EmployeeService
{
    public class EmployeeServices : IEmployeeServices
    {

        private readonly IUnitOfWork _unitOfWork;
        public EmployeeServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        public async Task UpdateEmployeeDetailsAsync(int employeeId, [FromBody] ProfileEdit profileEdit)
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
                IEnumerable<TBL_TRAVEL_TYPE> travelTypeData = await _unitOfWork.TravelTypeRepository.GetAllAsync();
                IEnumerable<TBL_COUNTRY> countryData = await _unitOfWork.CountryRepository.GetAllAsync();

                // Perform the join and projection
                var result = (from option in availableOptions
                              join request in requestData on option.RequestId equals request.RequestId
                              join mode in travelModeData on option.ModeId equals mode.ModeId
                              join sourceCountry in countryData on request.SourceCountry equals sourceCountry.CountryName
                              join destinationCountry in countryData on request.DestinationCountry equals destinationCountry.CountryName
                              join travelType in travelTypeData on request.TravelTypeId equals travelType.TravelTypeID
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
                                  SourceState = request.SourceState,
                                  SourceCountry = request.SourceCountry,
                                  SourceCountryCode = sourceCountry.CountryCode,
                                  DestinationCity = request.DestinationCity,
                                  DestinationState = request.DestinationState,
                                  DestinationCountry = request.DestinationCountry,
                                  DestinationCountryCode = destinationCountry.CountryCode,
                                  ModeId = option.ModeId,
                                  ModeName = mode.ModeName,
                                  TravelTypeId = request.TravelTypeId,
                                  TravelTypeName = travelType.TypeName
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
                IEnumerable<TBL_PROJECT_MAPPING> employeeProjectMap = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();

                var results = (from request in requestData
                               join statusApproval in statusApprovalMap on request.RequestId equals statusApproval.RequestId
                               join primarystatus in statusData on statusApproval.PrimaryStatusId equals primarystatus.StatusId
                               join secondarystatus in statusData on statusApproval.SecondaryStatusId equals secondarystatus.StatusId
                               join employeeProject in employeeProjectMap on statusApproval.EmpId equals employeeProject.EmpId
                               join project in projectData on employeeProject.ProjectId equals project.ProjectId
                               where secondarystatus.StatusCode == "PE" && statusApproval.EmpId == empId
                               select new PendingRequetsViewEmployee
                               {
                                   requestId = request.RequestId,
                                   projectName = project.ProjectName,
                                   reasonOfTravel = request.TripPurpose,
                                   destination = request.DestinationCity + ", " +request.DestinationCountry,
                                   dateOfTravel = request.DepartureDate
                               }).ToList();
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

        public async Task<IEnumerable<EmployeeViewReq>> GeRequestHistoryByEmpId(int empId)
        {
            try
            {
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> statusApprovalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_TRAVEL_TYPE> travelTypeData = await _unitOfWork.TravelTypeRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> projectMappingData = await _unitOfWork.ProjectMappingRepository.GetAllAsync();


                var result =  (from request in requestData
                              join statusApproval in statusApprovalData on request.RequestId equals statusApproval.RequestId
                              join primarystatus in statusData on statusApproval.PrimaryStatusId equals primarystatus.StatusId
                              join secondarystatus in statusData on statusApproval.SecondaryStatusId equals secondarystatus.StatusId
                              join projectMapping in projectMappingData on request.CreatedBy equals projectMapping.EmpId
                              join project in projectData on projectMapping.ProjectId equals project.ProjectId
                              join travelType in travelTypeData on request.TravelTypeId equals travelType.TravelTypeID
                              where request.CreatedBy == empId
                              && (secondarystatus.StatusCode == "CL" || primarystatus.StatusCode == "CL")
                               select new EmployeeViewReq
                              {
                                  RequestId = request.RequestId,
                                  ProjectCode = project.ProjectCode,
                                  ProjectName = project.ProjectName,
                                  TravelType = travelType.TypeName,
                                  ClosedDate = new DateOnly(statusApproval.date.Year, statusApproval.date.Month, statusApproval.date.Day),
                                  Status = "Closed"

                              }).ToList();
                return result;
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
                    join employee in employees on reqApproval.EmpId equals employee.EmpId
                    join projectMapping in projectMappings on employee.EmpId equals projectMapping.EmpId
                    join project in projects on projectMapping.ProjectId equals project.ProjectId
                    join primaryStatus in statusData on reqApproval.PrimaryStatusId equals primaryStatus.StatusId
                    join secondaryStatus in statusData on reqApproval.SecondaryStatusId equals secondaryStatus.StatusId
                    where employee.EmpId == employeeId
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
                    throw new FileNotFoundException($"No employees found who report to Manager ID {employeeId} with the specified criteria.");
                }
            }
            catch (Exception ex)
            {
                // Logging and rethrowing the exception
                Console.WriteLine($"An error occurred while getting employee details for Manager ID {employeeId}: {ex.Message}");
                throw;
            }
        }

    }
}
