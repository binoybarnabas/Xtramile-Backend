﻿using System.Linq.Expressions;
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
                IEnumerable<TBL_PROJECT_MAPPING> employeeProjectMappingData = await _unitOfWork.ProjectMappingRepository.GetAllAsync();

                var onGoingData = from employee in employeeData
                                  join requestStatus in requestStatusMappingData on employee.EmpId equals requestStatus.EmpId
                                  join request in requestData on requestStatus.RequestId equals request.RequestId
                                  join employeeProjectMapping in employeeProjectMappingData on employee.EmpId equals employeeProjectMapping.EmpId
                                  join project in projectData on employeeProjectMapping.ProjectId equals project.ProjectId
                                  join status in statusData on requestStatus.PrimaryStatusId equals status.StatusId
                                  where status.StatusCode == "OG"
                                  select new OngoingTravelAdmin
                                  {
                                      requestId = request.RequestId,
                                      ProjectCode = project.ProjectCode,
                                      ProjectName = project.ProjectName,
                                      FirstName = employee.FirstName,
                                      LastName = employee.LastName,
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
        
        public async Task<IEnumerable<RequestTableViewTravelAdmin>> GetIncomingRequests()
        {
            try
            {
                IEnumerable<TBL_REQ_APPROVE> approvalData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_PRIORITY> priorityData = await _unitOfWork.PriorityRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> projectMappingData = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_TRAVEL_TYPE> travelTypeData = await _unitOfWork.TravelTypeRepository.GetAllAsync();
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();

                var incomingRequests = from request in requestData
                                      join reqApprove in approvalData on request.RequestId equals reqApprove.RequestId
                                      join priority in priorityData on request.PriorityId equals priority.PriorityId into priorityGroup
                                      from priorityItem in priorityGroup.DefaultIfEmpty()
                                      join projectMapping in projectMappingData on request.CreatedBy equals projectMapping.EmpId
                                      join project in projectData on projectMapping.ProjectId equals project.ProjectId
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
                                      };


                // Execute the query and retrieve the results
                return incomingRequests.ToList();
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

    }
}
