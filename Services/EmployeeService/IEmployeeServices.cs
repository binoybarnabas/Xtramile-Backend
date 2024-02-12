        
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.EmployeeService
{
    public interface IEmployeeServices
    {

        public Task<IEnumerable<TBL_EMPLOYEE>> GetEmployeeAsync();

        public Task SetEmployeeAsync(TBL_EMPLOYEE employee);

        public Task<TBL_EMPLOYEE> GetEmployeeByIdAsync(int id);

        public Task<EmployeeInfo> GetEmployeeInfo(int id);

        public Task<EmployeeProfile> GetEmployeeProfileByIdAsync(int employeeId);

        public Task UpdateEmployeeDetailsAsync(int employeeId, ProfileEdit profileEdit);

        public Task<IEnumerable<OptionCard>> GetOptionsByReqId(int reqId);
        
        public Task<IEnumerable<PendingRequetsViewEmployee>> GetPendingRequestsByEmpId(int empId);
        public Task<IEnumerable<EmployeeOngoingRequest>> GetEmployeeOngoingRequestDetails(int employeeId);

        public Task<PagedEmployeeViewReqDto> GeRequestHistoryByEmpId(int empId, int pageIndex, int pageSize);

        public Task AddSelectedOptionForRequest(TBL_REQ_MAPPING option);

        public Task<EmployeeCurrentRequest> getEmployeeCurrentTravel(int empId);

        public Task<TBL_USER> updatePassword(string email, string newPassword);



    }
}
