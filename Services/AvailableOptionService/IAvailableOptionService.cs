using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using AvailableOption = XtramileBackend.Models.EntityModels.AvailableOption;

namespace XtramileBackend.Services.AvailableOptionService
{
    public interface IAvailableOptionServices
    {
        public Task<IEnumerable<AvailableOption>> GetAvailableOptionsAsync();
        public Task AddAvailableOptionAsync(AvailableOption availableOption);
        public Task<int> AddNewTravelOptionAsync(TravelOption travelOption);
        public Task UpdateFileIdOfOptionAsync(int fileId, int optionId);
        public Task<IEnumerable<TravelOption>> GetTravelOptionsByRequestIdAsync(int reqId,bool travelOption);
        public Task<string> AddAvailableTextOptionAsync(AvailableOptionText availableOption);
        public Task DeleteTravelOptions(int[] FileIds);
        public Task AddTravelAvailableOption(TravelOptionAPI travelOption, HttpContext httpContext);

    }
}
