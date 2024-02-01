using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.APIModels;

namespace XtramileBackend.Services.ManagerService
{
    public interface IReportingManagerService
    {
        public Task<List<EmployeeRequestDto>> GetEmployeeRequestsAsync([FromQuery] int managerId);

        public Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByRequestCodeAsync([FromQuery] int managerId);
        public Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByEmailAsync([FromQuery] int managerId);

        public Task<List<EmployeeRequestDto>> GetEmployeeRequestsSortByDateAsync([FromQuery] int managerId);
        public Task<List<EmployeeRequestDto>> GetEmployeeRequestsByDateAsync([FromQuery] int managerId, DateTime date);

        public Task<List<EmployeeRequestDto>> GetEmployeeRequestsByEmailAsync([FromQuery] int managerId, string email);
    }
}
