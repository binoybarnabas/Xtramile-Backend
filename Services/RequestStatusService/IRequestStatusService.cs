using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.RequestStatusService
{
    public interface IRequestStatusServices
    {
        public Task<IEnumerable<TBL_REQ_APPROVE>> GetRequestStatusesAsync();
        public Task AddRequestStatusAsync(TBL_REQ_APPROVE requestStatus);
        public Task<string> GetRequestStatusNameAsync(int requestId);
    }
}
