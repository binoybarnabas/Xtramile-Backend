using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.APIModels;

namespace XtramileBackend.Services.ManagerService
{
    public interface IReportingManagerService
    {
        public Task<List<EmployeeRequestDto>> GetEmployeeRequestsAsync([FromQuery] int managerId);

        public Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByRequestCodeAsync(int managerId);

        public Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByEmployeeNameAsync( int managerId);

        public Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByEmailAsync(int managerId);


        public Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByDateAsync(int managerId);
        public Task<List<EmployeeRequestDto>> GetEmployeeRequestsByDateAsync(int managerId, string date);

        public Task<List<EmployeeRequestDto>> GetEmployeeRequestsByEmailAsync(int managerId, string email);


        public Task<List<EmployeeRequestDto>> GetEmployeeRequestsByEmployeeNameAsync(int managerId,string employeeName);

        public Task<List<ManagerOngoingTravelRequest>> GetManagerOngoingTravelRequestDetails(int managerId);

    }
}
