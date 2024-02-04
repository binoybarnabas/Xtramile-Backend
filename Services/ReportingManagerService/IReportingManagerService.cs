using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.APIModels;

namespace XtramileBackend.Services.ManagerService
{
    public interface IReportingManagerService
    {
        //get all employees
        public Task<List<EmployeeRequestDto>> GetEmployeeRequestsAsync(int managerId);

        //sorting 
        public Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByRequestCodeAsync(int managerId);
        public Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByEmployeeNameAsync( int managerId);
        public Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByDateAsync(int managerId);

        //Get list by date
        public Task<List<EmployeeRequestDto>> GetEmployeeRequestsByDateAsync(int managerId, string date);

        //searching by name
        public Task<List<EmployeeRequestDto>> GetEmployeeRequestsByEmployeeNameAsync(int managerId,string employeeName);

        //ongoing requests
        public Task<IEnumerable<ManagerOngoingTravelRequest>> GetManagerOngoingTravelRequestDetails(int managerId);
        public Task<List<EmployeeRequestDto>> GetEmployeeRequestsClosedAsync(int managerId);

        public Task<TravelRequestEmployeeViewModel> GetEmployeeRequestDetail(int requestId);

    }
}
