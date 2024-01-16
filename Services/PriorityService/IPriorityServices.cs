using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.PriorityService
{
    public interface IPriorityServices
    {
        public IEnumerable<TBL_PRIORITY> GetPriorities();
        public void AddPriority(TBL_PRIORITY priority);
    }
}
