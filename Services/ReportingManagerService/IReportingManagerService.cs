using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.ManagerService
{
    public interface IReportingManagerService
    {
        //get all employees
        public Task<PagedEmployeeRequestDto> GetEmployeeRequestsAsync(int managerId, int offset, int pageSize);

        //sorting 
        public Task<PagedEmployeeRequestDto> GetEmployeeRequestsSortByRequestCodeAsync(int managerId, int offset, int pageSize);
        public Task<PagedEmployeeRequestDto> GetEmployeeRequestsSortByEmployeeNameAsync( int managerId, int offset, int pageSize);
        public Task<PagedEmployeeRequestDto> GetEmployeeRequestsSortByDateAsync(int managerId, int offset, int pageSize);

        //Get list by date
        public Task<PagedEmployeeRequestDto> GetEmployeeRequestsByDateAsync(int managerId, string date, int offset, int pageSize);

        //searching by name
        public Task<PagedEmployeeRequestDto> GetEmployeeRequestsByEmployeeNameAsync(int managerId,string employeeName, int offset, int pageSize);

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

        public Task<IEnumerable<RequestNotification>> getManagerRequestNotification(int empId);

    }
}
