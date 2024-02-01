using System.Linq.Expressions;
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

        public async Task<IEnumerable<OngoingTravelAdmin>> OnGoingTravel() {

            try
            {
                IEnumerable<TBL_EMPLOYEE> employeeData = await _unitOfWork.EmployeeRepository.GetAllAsync();
                IEnumerable<TBL_STATUS> statusData = await _unitOfWork.StatusRepository.GetAllAsync();
                IEnumerable<TBL_REQ_APPROVE> requestOptionMappingData = await _unitOfWork.RequestStatusRepository.GetAllAsync();
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();

                var onGoingData = from employee in employeeData
                                  join requestOption in requestOptionMappingData on employee.EmpId equals requestOption.EmpId
                                  join request in requestData on requestOption.RequestId equals request.RequestId
                                  join project in projectData on request.ProjectId equals project.ProjectId
                                  join status in statusData on requestOption.PrimaryStatusId equals status.StatusId
                                  where status.StatusCode == "OG"
                                  select new OngoingTravelAdmin
                                  {
                                      RequestCode = request.RequestCode,
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
    }
}
