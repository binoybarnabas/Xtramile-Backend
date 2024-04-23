using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.RequestStatusService
{
    public interface IRequestStatusServices
    {
        public Task<IEnumerable<RequestApprove>> GetRequestStatusesAsync();
        public Task AddRequestStatusAsync(RequestApprove requestStatus);
        public Task<string> GetRequestStatusNameAsync(int requestId);
    }
}
