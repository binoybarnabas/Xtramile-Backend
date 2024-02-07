using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;

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

        //closed travel requets
        public Task<PagedEmployeeRequestDto> GetEmployeeRequestsClosedAsync(int managerId, int offset, int pageSize);

        //request detail of an employee 
        public Task<TravelRequestEmployeeViewModel> GetEmployeeRequestDetail(int requestId);

        public Task<bool> UpdateRequestPriorityAndStatus(UpdatePriorityAndStatusModel updatePriorityAndStatus);

        public Task<bool> CancelRequest(ManagerCancelRequest managerCancelRequest);
        public Task PostReasonForCancellation(TBL_REASON reason, int reqId);

         
        // forwarded travel requests
        public Task<PagedEmployeeRequestDto> GetEmployeeRequestsForwardedAsync(int managerId, int offset, int pageSize);

        
    }
}
