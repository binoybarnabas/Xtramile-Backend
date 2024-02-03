using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.StatusService
{
    public interface IStatusServices

    {
        public Task<IEnumerable<TBL_STATUS>> GetAllStatusAsync();
        public Task AddStatusAsync(TBL_STATUS status);
        public Task<int> GetStatusIdByCode(string statusCode);

    }
}

