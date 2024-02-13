using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.TravelModeService
{
    public interface ITravelModeService
    {
        public Task<IEnumerable<TBL_TRAVEL_MODE>> GetTravelModeAsync();

        public Task SetTravelModeAsync(TBL_TRAVEL_MODE travelMode);

        public Task<string> GetTravelModeByIdAsync(int id);

    }
}
