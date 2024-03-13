using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.StatusService
{
    public interface IStatusServices

    {
        public Task<IEnumerable<TBL_STATUS>> GetAllStatusAsync();
        public Task AddStatusAsync(TBL_STATUS status);
        public Task<int> GetStatusIdByCode(string statusCode);
        public Task<int> GetStatusIdByStatusCodeAsync(string statusCode);

        public Task<string> GetPrimaryStatusByRequestIdAsync(int reqId);
        public string GetStatusName(int primaryStatusId, int secondaryStatusId);

    }
}

