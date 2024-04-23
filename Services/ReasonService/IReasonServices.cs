using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.ReasonService
{
    public interface IReasonServices

    {
        public Task<IEnumerable<Reason>> GetAllReasonsAsync();
        public Task AddReasonAsync(Reason reason);

    }
}

