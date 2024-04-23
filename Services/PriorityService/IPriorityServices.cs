using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.PriorityService
{
    public interface IPriorityServices
    {
        public Task<IEnumerable<Priority>> GetPrioritiesAsync();
        public Task AddPriorityAsync(Priority priority);
    }
}
