using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.AvailableOptionService
{
    public interface IAvailableOptionServices
    {
        public Task<IEnumerable<TBL_AVAIL_OPTION>> GetAvailableOptionsAsync();
        public Task AddAvailableOptionAsync(TBL_AVAIL_OPTION availableOption);
        public Task<int> AddNewTravelOptionAsync(TBL_TRAVEL_OPTION travelOption);
        public Task UpdateFileIdOfOptionAsync(int fileId, int optionId);
        public Task<IEnumerable<TBL_TRAVEL_OPTION>> GetTravelOptionsByRequestIdAsync(int reqId,bool travelOption);
        public Task<string> AddAvailableTextOptionAsync(AvailableOption availableOption);

    }
}
