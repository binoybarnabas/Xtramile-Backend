using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.PriorityService
{
    public interface IPriorityServices
    {
        public Task<IEnumerable<TBL_PRIORITY>> GetPrioritiesAsync();
        public Task AddPriorityAsync(TBL_PRIORITY priority);
    }
}
