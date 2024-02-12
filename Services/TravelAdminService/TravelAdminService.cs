using Azure.Core;
using OfficeOpenXml;
using System.Linq.Expressions;
using System.Threading.Tasks.Dataflow;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.TravelAdminService
{
    public class TravelAdminService : ITravelAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TravelAdminService(IUnitOfWork unitOfWork) { 
        _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// To get a list of all the ongoing trips that can be seen by the travel admin.
        /// </summary>
        /// <returns>
        /// return a list of data which consists of ongoingTrips which contains information like requestId, Project code
        /// project name,first name and last name of the employee source city and destination city.
        /// </returns>
        public async Task<IEnumerable<OngoingTravelAdmin>> OnGoingTravel()
        {

            try
            {
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> requestStatusMappingData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();

                var latestStatusApprovals = requestStatusMappingData
    .GroupBy(approval => approval.RequestId)
    .Select(group => group.OrderByDescending(approval => approval.date).First());

                var onGoingData = from employee in employeeData
                                  join requestStatus in latestStatusApprovals on employee.EmpId equals requestStatus.EmpId
                                  join request in requestData on requestStatus.RequestId equals request.RequestId
                                  join project in projectData on request.ProjectId equals project.ProjectId
                                  join status in statusData on requestStatus.PrimaryStatusId equals status.StatusId
                                  where status.StatusCode == "OG"
                                  select new OngoingTravelAdmin
                                  {
                                      requestId = request.RequestId,
                                      ProjectCode = project.ProjectCode,
                                      ProjectName = project.ProjectName,
                                      Name = employee.FirstName+ " "+employee.LastName,
                                      SourceCity = request.SourceCity,
                                      DestinationCity = request.DestinationCity
                                  };

                return onGoingData.ToList();
            }

            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting on Going travel requests: {ex.Message}");
                throw;

            }
        }

        /// <summary>
        ///This LINQ query retrieves specific information from multiple tables, including requests, approvals, priorities, projects,
        // travel types, employees, and statuses. It applies joins and filters based on certain conditions, then selects and
        // projects specific fields to create a result set with request ID, employee name, project code, creation date,
        // travel type name, priority name, and status name.This returning object is used to display all incoming requests for a travel admin. 
        /// </summary>
        /// <returns>List of objects containing the requested data.</returns>
        public async Task<RequestTableViewTravelAdminPaged> GetIncomingRequests(int pageIndex, int pageSize)

        {
            try
            {
                IEnumerable<TBL_REQ_APPROVE> approvalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_PRIORITY> priorityData = await _unitOfWork.PriorityRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_TRAVEL_TYPE> travelTypeData = await _unitOfWork.TravelTypeRepository.GetAllAsync();
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();

                var latestApprovals = approvalData.GroupBy(approval => approval.RequestId)
                                   .Select(group => group.OrderByDescending(approval => approval.date).FirstOrDefault());


                var incomingRequests = ( from request in requestData
                                       join reqApprove in latestApprovals on request.RequestId equals reqApprove.RequestId
                                       join priority in priorityData on request.PriorityId equals priority.PriorityId into priorityGroup
                                       from priorityItem in priorityGroup.DefaultIfEmpty()
                                       join project in projectData on request.ProjectId equals project.ProjectId
                                       join travelType in travelTypeData on request.TravelTypeId equals travelType.TravelTypeID
                                       join employee in employeeData on request.CreatedBy equals employee.EmpId
                                       join approver in employeeData on reqApprove.EmpId equals approver.EmpId
                                       join status in statusData on reqApprove.PrimaryStatusId equals status.StatusId
                                       where (reqApprove.PrimaryStatusId == 1) || (reqApprove.PrimaryStatusId == 12 && approver.RoleId == 2)
                                       select new RequestTableViewTravelAdmin
                                       {
                                           RequestId = request.RequestId,
                                           EmployeeName = $"{employee.FirstName} {employee.LastName}",
                                           ProjectCode = project.ProjectCode,
                                           CreatedOn = request.CreatedOn,
                                           TravelTypeName = travelType.TypeName,
                                           PriorityName = priorityItem?.PriorityName ?? "Null",// Using ?. to handle null in case of no priority
                                           StatusName = status.StatusName
                                       } ).ToList();


                // Execute the query and retrieve the results

                // Pagination
                var totalCount = incomingRequests.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                var pagedIncomingRequests = incomingRequests.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                // Return paged result
                return new RequestTableViewTravelAdminPaged
                {
                    TravelRequest = pagedIncomingRequests,
                    TotalPages = totalPages,
                    PageCount = totalCount,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting data for incoming requests: {ex.Message}");
                throw;

            }

        }

        /// <summary>
        /// Retrieves the selected option details for a specific request ID.
        /// </summary>
        /// <param name="reqId"></param>
        /// <returns>An instance of OptionCard representing the selected option details, or null if not found.</returns>
        public async Task<OptionCard> GetSelectedOptionFromEmployee(int reqId)
        {
            try
            {
                // Fetch data from repositories
                IEnumerable<TBL_REQ_MAPPING> reqMappingData = await _unitOfWork.RequestMappingRepository.GetAllAsync();
                IEnumerable<TBL_AVAIL_OPTION> availableOptions = await _unitOfWork.AvailableOptionRepository.GetAllAsync();
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_TRAVEL_MODE> travelModeData = await _unitOfWork.TravelModeRepository.GetAllAsync();
                IEnumerable<TBL_TRAVEL_TYPE> travelTypeData = await _unitOfWork.TravelTypeRepository.GetAllAsync();
                IEnumerable<TBL_COUNTRY> countryData = await _unitOfWork.CountryRepository.GetAllAsync();

                // Perform the join and projection
                var result = (from reqMapping in reqMappingData
                              join option in availableOptions on reqMapping.OptionId equals option.OptionId
                              join request in requestData on option.RequestId equals request.RequestId
                              join mode in travelModeData on option.ModeId equals mode.ModeId
                              join sourceCountry in countryData on request.SourceCountry equals sourceCountry.CountryName
                              join destinationCountry in countryData on request.DestinationCountry equals destinationCountry.CountryName
                              join travelType in travelTypeData on request.TravelTypeId equals travelType.TravelTypeID
                              where reqMapping.RequestId == reqId
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
                                  TravelTypeName = travelType.TypeName,
                              })
                              .FirstOrDefault(); // Return the first matching result or null

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting option : {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a collection of travel requests based on a specified status code,
        /// returning the last row of each unique request ID with the specified status.
        /// </summary>
        /// <param name="statusCode">The status code to filter the requests.</param>
        /// <returns>A collection of RequestTableViewTravelAdmin representing the last row of each unique request ID with the specified status.</returns>
        public async Task<IEnumerable<RequestTableViewTravelAdmin>> GetTravelRequests(string statusCode)
        {
            IEnumerable<TBL_REQ_APPROVE> approvalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
            IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
            IEnumerable<TBL_PRIORITY> priorityData = await _unitOfWork.PriorityRepository.GetAllAsync();
            IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
            IEnumerable<TBL_TRAVEL_TYPE> travelTypeData = await _unitOfWork.TravelTypeRepository.GetAllAsync();
            IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();
            IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();

            var latestStatusApprovals = approvalData
                .GroupBy(approval => approval.RequestId)
                .Select(group => group.OrderByDescending(approval => approval.date).First());

            var result = (from latestApproval in latestStatusApprovals
                          join requests in requestData on latestApproval.RequestId equals requests.RequestId
                          join priority in priorityData on requests.PriorityId equals priority.PriorityId
                          join status in statusData on latestApproval.PrimaryStatusId equals status.StatusId
                          join travelType in travelTypeData on requests.TravelTypeId equals travelType.TravelTypeID
                          join employee in employeeData on requests.CreatedBy equals employee.EmpId
                          join project in projectData on requests.ProjectId equals project.ProjectId
                          where status.StatusCode == statusCode
                          select new RequestTableViewTravelAdmin
                          {
                              RequestId = requests.RequestId,
                              EmployeeName = employee.FirstName + " " + employee.LastName,
                              ProjectCode = project.ProjectCode,
                              CreatedOn = requests.CreatedOn,
                              TravelTypeName = travelType.TypeName,
                              PriorityName = priority.PriorityName,
                              ApprovalDate = latestApproval.date
                          }).ToList();

            return result;
        }

        /// <summary>
        /// Get the detail of the request based on a request id 
        /// This shows the request related information for a particular request raised by an employee
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public async Task<TravelRequestEmployeeViewModel> GetEmployeeRequestDetail(int requestId)
        {
            try
            {

                IEnumerable<TBL_EMPLOYEE> employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> reqApprovals = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_REQUEST> travelRequests = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_TRAVEL_TYPE> travelTypes = await _unitOfWork.TravelTypeRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projects = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_DEPARTMENT> departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
                var employeeRequestDetail = (from employee in employees
                                             join travelRequest in travelRequests on employee.EmpId equals travelRequest.CreatedBy
                                             join travelType in travelTypes on travelRequest.TravelTypeId equals travelType.TravelTypeID
                                             join project in projects on travelRequest.ProjectId equals project.ProjectId
                                             join department in departments on project.DepartmentId equals department.DepartmentId
                                             join reportsToEmployee in employees on employee.ReportsTo equals reportsToEmployee.EmpId
                                             where travelRequest.RequestId == requestId
                                             select new TravelRequestEmployeeViewModel
                                             {
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
        /// Sort incoming request 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="employeeName"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<RequestTableViewTravelAdminPaged> GetIncomingRequestsSorted(int pageIndex, int pageSize, bool employeeName, bool date)
        {
            try
            {
                List<RequestTableViewTravelAdmin> incomingRequests =new List<RequestTableViewTravelAdmin> ();

                if (employeeName)
                {
                    incomingRequests = (List<RequestTableViewTravelAdmin>)await GetIncomingRequestSoryByDate();
                }
                else if (date)
                {
                    incomingRequests = (List<RequestTableViewTravelAdmin>)await GetIncomingRequestSortByEmployeeName();
                }
                else
                {
                    GetIncomingRequests(pageIndex, pageSize);
                }

                // Pagination
                var totalCount = incomingRequests.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                var pagedIncomingRequests = incomingRequests.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                // Return paged result
                return new RequestTableViewTravelAdminPaged
                {
                    TravelRequest = pagedIncomingRequests,
                    TotalPages = totalPages,
                    PageCount = totalCount,
                };

            }
            catch (Exception ex)
            {
                //  Logging and rethrowing the exception
                Console.WriteLine($"An error occured while getting the incoming requests sorted: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Sort the request based on the request date
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<RequestTableViewTravelAdmin>> GetIncomingRequestSoryByDate()
        {
            IEnumerable<TBL_REQ_APPROVE> approvalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
            IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
            IEnumerable<TBL_PRIORITY> priorityData = await _unitOfWork.PriorityRepository.GetAllAsync();
            IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
            IEnumerable<TBL_TRAVEL_TYPE> travelTypeData = await _unitOfWork.TravelTypeRepository.GetAllAsync();
            IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();
            IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();

            var latestStatusApprovals = approvalData
    .GroupBy(approval => approval.RequestId)
    .Select(group => group.OrderByDescending(approval => approval.date).First());

            // Fetch required data from repositories
            var incomingRequests = (from request in requestData
                                    join reqApprove in latestStatusApprovals on request.RequestId equals reqApprove.RequestId
                                    join priority in priorityData on request.PriorityId equals priority.PriorityId into priorityGroup
                                    from priorityItem in priorityGroup.DefaultIfEmpty()
                                    join project in projectData on request.ProjectId equals project.ProjectId
                                    join travelType in travelTypeData on request.TravelTypeId equals travelType.TravelTypeID
                                    join employee in employeeData on request.CreatedBy equals employee.EmpId
                                    join approver in employeeData on reqApprove.EmpId equals approver.EmpId
                                    join status in statusData on reqApprove.PrimaryStatusId equals status.StatusId
                                    where (reqApprove.PrimaryStatusId == 1) || (reqApprove.PrimaryStatusId == 12 && approver.RoleId == 2)
                                    orderby request.CreatedOn
                                    select new RequestTableViewTravelAdmin
                                    {
                                        RequestId = request.RequestId,
                                        EmployeeName = $"{employee.FirstName} {employee.LastName}",
                                        ProjectCode = project.ProjectCode,
                                        CreatedOn = request.CreatedOn,
                                        TravelTypeName = travelType.TypeName,
                                        PriorityName = priorityItem?.PriorityName ?? "Null",// Using ?. to handle null in case of no priority
                                        StatusName = status.StatusName
                                    }).ToList();
            return incomingRequests;
        }



        /// <summary>
        /// Sort the incoming request based on employeeName
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<RequestTableViewTravelAdmin>> GetIncomingRequestSortByEmployeeName()
        {
            IEnumerable<TBL_REQ_APPROVE> approvalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
            IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
            IEnumerable<TBL_PRIORITY> priorityData = await _unitOfWork.PriorityRepository.GetAllAsync();
            IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
            IEnumerable<TBL_TRAVEL_TYPE> travelTypeData = await _unitOfWork.TravelTypeRepository.GetAllAsync();
            IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();
            IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();

            var latestStatusApprovals = approvalData
    .GroupBy(approval => approval.RequestId)
    .Select(group => group.OrderByDescending(approval => approval.date).First());
            // Fetch required data from repositories
            var incomingRequests = (from request in requestData
                                    join reqApprove in latestStatusApprovals on request.RequestId equals reqApprove.RequestId
                                    join priority in priorityData on request.PriorityId equals priority.PriorityId into priorityGroup
                                    from priorityItem in priorityGroup.DefaultIfEmpty()
                                    join project in projectData on request.ProjectId equals project.ProjectId
                                    join travelType in travelTypeData on request.TravelTypeId equals travelType.TravelTypeID
                                    join employee in employeeData on request.CreatedBy equals employee.EmpId
                                    join approver in employeeData on reqApprove.EmpId equals approver.EmpId
                                    join status in statusData on reqApprove.PrimaryStatusId equals status.StatusId
                                    where (reqApprove.PrimaryStatusId == 1) || (reqApprove.PrimaryStatusId == 12 && approver.RoleId == 2)
                                    orderby employee.FirstName, employee.LastName
                                    select new RequestTableViewTravelAdmin
                                    {
                                        RequestId = request.RequestId,
                                        EmployeeName = $"{employee.FirstName} {employee.LastName}",
                                        ProjectCode = project.ProjectCode,
                                        CreatedOn = request.CreatedOn,
                                        TravelTypeName = travelType.TypeName,
                                        PriorityName = priorityItem?.PriorityName ?? "Null",// Using ?. to handle null in case of no priority
                                        StatusName = status.StatusName
                                    }).ToList();
            return incomingRequests;
        }


        /// <summary>
        /// get all the requests based on a given date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RequestTableViewTravelAdmin>> GetEmployeeRequestsByDateAsync(string date)
        {
            IEnumerable<TBL_REQ_APPROVE> approvalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
            IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
            IEnumerable<TBL_PRIORITY> priorityData = await _unitOfWork.PriorityRepository.GetAllAsync();
            IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
            IEnumerable<TBL_TRAVEL_TYPE> travelTypeData = await _unitOfWork.TravelTypeRepository.GetAllAsync();
            IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();
            IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();

            var latestStatusApprovals = approvalData
    .GroupBy(approval => approval.RequestId)
    .Select(group => group.OrderByDescending(approval => approval.date).First());
            // Fetch required data from repositories
            var incomingRequests = (from request in requestData
                                    join reqApprove in latestStatusApprovals on request.RequestId equals reqApprove.RequestId
                                    join priority in priorityData on request.PriorityId equals priority.PriorityId into priorityGroup
                                    from priorityItem in priorityGroup.DefaultIfEmpty()
                                    join project in projectData on request.ProjectId equals project.ProjectId
                                    join travelType in travelTypeData on request.TravelTypeId equals travelType.TravelTypeID
                                    join employee in employeeData on request.CreatedBy equals employee.EmpId
                                    join approver in employeeData on reqApprove.EmpId equals approver.EmpId
                                    join status in statusData on reqApprove.PrimaryStatusId equals status.StatusId
                                    where ((reqApprove.PrimaryStatusId == 1) || (reqApprove.PrimaryStatusId == 12 && approver.RoleId == 2)) && request.CreatedOn.Date == DateTime.ParseExact(date, "yyyy-MM-dd", null)
                                    
                                    select new RequestTableViewTravelAdmin
                                    {
                                        RequestId = request.RequestId,
                                        EmployeeName = $"{employee.FirstName} {employee.LastName}",
                                        ProjectCode = project.ProjectCode,
                                        CreatedOn = request.CreatedOn,
                                        TravelTypeName = travelType.TypeName,
                                        PriorityName = priorityItem?.PriorityName ?? "High",// Using ?. to handle null in case of no priority
                                        StatusName = status.StatusName
                                    }).ToList();
            return incomingRequests;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RequestTableViewTravelAdmin>> GetEmployeeRequestsByEmployeeNameAsync(string employeeName)
        {
            IEnumerable<TBL_REQ_APPROVE> approvalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
            IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
            IEnumerable<TBL_PRIORITY> priorityData = await _unitOfWork.PriorityRepository.GetAllAsync();
            IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
            IEnumerable<TBL_TRAVEL_TYPE> travelTypeData = await _unitOfWork.TravelTypeRepository.GetAllAsync();
            IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();
            IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();

            var latestStatusApprovals = approvalData
    .GroupBy(approval => approval.RequestId)
    .Select(group => group.OrderByDescending(approval => approval.date).First());
            // Fetch required data from repositories
            var incomingRequests = (from request in requestData
                                    join reqApprove in latestStatusApprovals on request.RequestId equals reqApprove.RequestId
                                    join priority in priorityData on request.PriorityId equals priority.PriorityId into priorityGroup
                                    from priorityItem in priorityGroup.DefaultIfEmpty()
                                    join project in projectData on request.ProjectId equals project.ProjectId
                                    join travelType in travelTypeData on request.TravelTypeId equals travelType.TravelTypeID
                                    join employee in employeeData on request.CreatedBy equals employee.EmpId
                                    join approver in employeeData on reqApprove.EmpId equals approver.EmpId
                                    join status in statusData on reqApprove.PrimaryStatusId equals status.StatusId
                                    where ((reqApprove.PrimaryStatusId == 1) || (reqApprove.PrimaryStatusId == 12 && approver.RoleId == 2))
                                     && (employee.FirstName + " " + employee.LastName).Contains(employeeName)
                                    select new RequestTableViewTravelAdmin
                                    {
                                        RequestId = request.RequestId,
                                        EmployeeName = $"{employee.FirstName} {employee.LastName}",
                                        ProjectCode = project.ProjectCode,
                                        CreatedOn = request.CreatedOn,
                                        TravelTypeName = travelType.TypeName,
                                        PriorityName = priorityItem?.PriorityName ?? "High",// Using ?. to handle null in case of no priority
                                        StatusName = status.StatusName
                                    }).ToList();
            return incomingRequests;
        }



    }
}
        /// <summary>
        /// Generates an Excel report summarizing the count of travel modes used in a specified month.
        /// </summary>
        /// <param name="monthName">The name of the month for which the report is generated (e.g., "January").</param>
        /// <returns>A byte array representing the Excel file containing the mode count report.</returns>
        /// <remarks>
        /// This method loads data from the database into memory using asynchronous operations,
        /// performs LINQ queries to calculate the count of each travel mode used in the specified month,
        /// and generates an Excel report with the mode names and their respective counts.
        /// </remarks>
        public async Task<byte[]> GenerateModeCountFromMonthReport(string monthName)
        {
            // Load tables into memory
            try
            {
                IEnumerable<TBL_REQUEST> requests = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> approves = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_TRAVEL_MODE> modes = await _unitOfWork.TravelModeRepository.GetAllAsync();

                DateTime monthDate = DateTime.ParseExact(monthName, "MMMM", System.Globalization.CultureInfo.InvariantCulture);
                int month = monthDate.Month;

                // Filter requests created in the specified month
                var filteredRequests = requests.Where(r => r.CreatedOn.Month == month);

                // Join with approved requests in TBL_REQ_APPROVE
                var approvedRequests = from request in filteredRequests
                                       join approval in approves
                                       on request.RequestId equals approval.RequestId
                                       where approval.PrimaryStatusId == 3
                                       select new
                                       {
                                           request.ModeId
                                       };

                // Count how many times each mode id was used in a single month
                var modeCounts = approvedRequests.GroupBy(r => r.ModeId)
                                                  .Select(g => new
                                                  {
                                                      ModeId = g.Key,
                                                      Count = g.Count()
                                                  });

                // Get ModeName from TBL_TRAVEL_MODE using mode id in TBL_REQUEST
                var modeDetails = from modeCount in modeCounts
                                  join mode in modes
                                  on modeCount.ModeId equals mode.ModeId
                                  select new
                                  {
                                      ModeName = mode.ModeName,
                                      Count = modeCount.Count
                                  };

                // Create Excel package
                using (var package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Monthly Mode Report");

                    // Add headers
                    worksheet.Cells[1, 1].Value = "Month";
                    worksheet.Cells[1, 2].Value = monthName;
                    worksheet.Cells[2, 1].Value = "Mode Name";
                    worksheet.Cells[2, 2].Value = "Count";


                    // Populate data
                    int row = 3;
                    foreach (var item in modeDetails)
                    {
                        worksheet.Cells[row, 1].Value = item.ModeName;
                        worksheet.Cells[row, 2].Value = item.Count;
                        row++;
                    }

                    // Convert to byte array
                    return package.GetAsByteArray();
                }

            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine($"An error while generating report: {ex.Message}");
                return null;
            }
        }
        /// <summary>
        /// Generates an Excel report summarizing the count of travel modes used in requests associated with a specified project.
        /// </summary>
        /// <param name="projectId">The ID of the project for which the report is generated.</param>
        /// <returns>A byte array representing the Excel file containing the mode count report.</returns>
        /// <remarks>
        /// This method loads data from the database into memory using asynchronous operations,
        /// filters requests based on the provided project ID, retrieves the project name,
        /// performs LINQ queries to calculate the count of each travel mode used in the project,
        /// and generates an Excel report with the mode names and their respective counts.
        /// </remarks>
        public async Task<byte[]> GenerateModeCountFromProjectIdExcelReport(int projectId)
        {
            // Load tables into memory
            try
            {
                IEnumerable<TBL_REQUEST> requests = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> approves = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_TRAVEL_MODE> modes = await _unitOfWork.TravelModeRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projects = await _unitOfWork.ProjectRepository.GetAllAsync();

                // Filter requests based on projectId
                var filteredRequests = requests.Where(r => r.ProjectId == projectId);

                // To get the project name 
                var projectName = (from request in filteredRequests
                                   join project in projects
                                   on request.ProjectId equals project.ProjectId
                                   select project.ProjectName).FirstOrDefault();

                // Join with approved requests in TBL_REQ_APPROVE
                var approvedRequests = from request in filteredRequests
                                       join approval in approves
                                       on request.RequestId equals approval.RequestId
                                       where approval.PrimaryStatusId == 3
                                       select new
                                       {
                                           request.ModeId
                                       };

                // Count how many times each mode id was used in a single project
                var modeCounts = approvedRequests.GroupBy(r => r.ModeId)
                                                  .Select(g => new
                                                  {
                                                      ModeId = g.Key,
                                                      Count = g.Count()
                                                  });

                // Get ModeName from TBL_TRAVEL_MODE using mode id in TBL_REQUEST
                var modeDetails = from modeCount in modeCounts
                                  join mode in modes
                                  on modeCount.ModeId equals mode.ModeId
                                  select new
                                  {
                                      ModeName = mode.ModeName,
                                      Count = modeCount.Count
                                  };

                // Create Excel package
                using (var package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Project based Travel Mode Report");

                    // Add headers
                    worksheet.Cells[1, 1].Value = "Project Name";
                    worksheet.Cells[1, 2].Value = projectName;
                    worksheet.Cells[2, 1].Value = "Mode Name";
                    worksheet.Cells[2, 2].Value = "Count";

                    // Populate data
                    int row = 3;
                    foreach (var item in modeDetails)
                    {
                        worksheet.Cells[row, 1].Value = item.ModeName;
                        worksheet.Cells[row, 2].Value = item.Count;
                        row++;
                    }

                    // Convert to byte array
                    return package.GetAsByteArray();
                }

            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine($"An error while generating report: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Retrieves the count of approved requests grouped by month based on their departure dates.
        /// </summary>
        /// <returns>A dictionary where keys represent the month names and values represent the count of approved requests in each month.</returns>
        /// <remarks>
        /// This method loads data from the database into memory using asynchronous operations,
        /// retrieves approved requests from the repository and joins them with request statuses,
        /// groups the requests by month based on their departure dates,
        /// fills in missing months with zero counts,
        /// and returns a dictionary containing the count of approved requests for each month.
        /// </remarks>
        public async Task<Dictionary<string, int>> GetRequestsByMonth()
        {
            try
            {
                IEnumerable<TBL_REQUEST> requests = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> approves = await _unitOfWork.RequestStatusRepository.GetAllAsync();

                var approvedRequests = from req in requests
                                       join app in approves
                                       on req.RequestId equals app.RequestId
                                       where app.PrimaryStatusId == 3
                                       select req;

                var requestsByMonth = approvedRequests
                    .GroupBy(r => r.DepartureDate.ToString("MMMM"))// using DepartureDate to filter requests to corresponding months
                    .ToDictionary(g => g.Key, g => g.Count());

                var allMonths = Enumerable.Range(1, 12)
                    .Select(i => new DateTime(2000, i, 1).ToString("MMMM"))// to generate all months
                    .ToArray();

                foreach (var month in allMonths)
                {
                    if (!requestsByMonth.ContainsKey(month))
                    {
                        requestsByMonth.Add(month, 0);
                    }
                }

                return requestsByMonth;
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine($"An error while generating report: {ex.Message}");
                return null;
            }


        }
    }
}

