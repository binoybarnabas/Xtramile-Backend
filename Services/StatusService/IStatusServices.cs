using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.StatusService
{
    public interface IStatusServices

    {
        public Task<IEnumerable<Status>> GetAllStatusAsync();
        public Task AddStatusAsync(Status status);
        public Task<int> GetStatusIdByCode(string statusCode);
        public Task<int> GetStatusIdByStatusCodeAsync(string statusCode);

        public Task<string> GetPrimaryStatusByRequestIdAsync(int reqId);
        public string GetStatusName(int primaryStatusId, int secondaryStatusId);

    }
}

