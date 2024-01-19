using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.ReasonService
{
    public interface IReasonServices

    {
        public Task<IEnumerable<TBL_REASON>> GetAllReasonsAsync();
        public Task AddReasonAsync(TBL_REASON reason);

    }
}

