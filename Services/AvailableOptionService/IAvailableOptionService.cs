using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.AvailableOptionService
{
    public interface IAvailableOptionServices
    {
        public Task<IEnumerable<TBL_AVAIL_OPTION>> GetAvailableOptionsAsync();
        public Task AddAvailableOptionAsync(TBL_AVAIL_OPTION availableOption);
    }
}
