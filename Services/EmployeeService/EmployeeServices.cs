using Microsoft.AspNetCore.Mvc;
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

        public async Task<EmployeeProfile> GetEmployeeProfileByIdAsync(int employeeId)
        {
            try
            {
                IEnumerable<TBL_EMPLOYEE> employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> projectMappings = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projects = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_DEPARTMENT> departments = await _unitOfWork.DepartmentRepository.GetAllAsync();

                var result = (
                            from employee in employees
                            join mapping in projectMappings on employee.EmpId equals mapping.EmpId
                            join project in projects on mapping.ProjectId equals project.ProjectId
                            join department in departments on project.DepartmentId equals department.DepartmentId
                            join reportsToEmployee in employees on employee.ReportsTo equals reportsToEmployee.EmpId into reportsToJoin
                            from reportsToEmployee in reportsToJoin.DefaultIfEmpty() // Perform left join
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


                if (result != null)
                {
                    return result;
                }
                else
                {
                    throw new FileNotFoundException($"Employee with ID {employeeId} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting employee profile by ID: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateEmployeeDetailsAsync(int employeeId, [FromBody] ProfileEdit profileEdit)
        {
            TBL_EMPLOYEE employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);

            if (employee != null)
            {
                if (profileEdit != null)
                {
                    if (profileEdit.ContactNumber != null)
                    {
                        employee.ContactNumber = profileEdit.ContactNumber;
                    }

                    if (profileEdit.Address != null)
                    {
                        employee.Address = profileEdit.Address;
                    }

                    await _unitOfWork.SaveChangesAsyn();
                }
            }
            else
            {
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
                              join mode in travelModeData on request.ModeId equals mode.ModeId
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
                                  ModeId = request.ModeId,
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
    }
}
