
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.EmployeeService
{
    public interface IEmployeeServices
    {

        public Task<IEnumerable<Employee>> GetEmployeeAsync();

        public Task SetEmployeeAsync(Employee employee);

        public Task<Employee> GetEmployeeByIdAsync(int id);

        public Task<EmployeeInfo> GetEmployeeInfo(int id);

        public Task<EmployeeProfile> GetEmployeeProfileByIdAsync(int employeeId, HttpContext httpContext);

        public Task UpdateEmployeeDetailsAsync(int employeeId, ProfileEdit profileEdit);

        public Task<IEnumerable<OptionCard>> GetOptionsByReqId(int reqId);

        public Task<PageinatedResult<PendingRequetsViewEmployee>> GetPendingRequestsByEmpId(int empId, int pageNumber, int itemsPerPage);
        public Task<PageinatedResult<EmployeeOngoingRequest>> GetEmployeeOngoingRequestDetails(int employeeId, int pageNumber, int itemsPerPage);

        public Task<PagedEmployeeViewReqDto> GeRequestHistoryByEmpId(int empId, int pageIndex, int pageSize);
        
        public Task AddSelectedOptionForRequest(TBL_REQ_MAPPING option);

        public Task<IEnumerable<EmployeeCurrentRequest>> getEmployeeCurrentTravel(int empId);

        public Task<User> updatePassword(string email, string newPassword);

        public Task<IEnumerable<DashboardUpcomingTrip>> GetEmployeeDashboardUpcomingTripByIdAsync(int employeeId);
        public Task<IEnumerable<DashboardEmployeeprogress>> GetEmployeeDashboardProgressAsync(int employeeId);

        public Task<IEnumerable<RequestNotification>> GetEmployeeRequestNotificationsAsync(int empId);
        public Task<object> GetCompletedTrips(int empId);

        public Task<bool> EmployeeCancelRequest(int requestId, int empId);

        public Task SubmitSelectedTravelOptionAsync(TravelOptionMap travelOption);

        public Task<IEnumerable<PendingRequetsViewEmployee>> GetFilteredPendingRequestsByEmpId(int empId, string primaryStatusCode, string secondaryStatusCode);
        public Task<FileMetaData> AddEmployeeProfilePicture(IFormFile profilePicture, int employeeId, HttpContext context);
        public Task UpdateProfilePicture(IFormFile profilePicture, int employeeId, HttpContext httpContext);



    }
}
