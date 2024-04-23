using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.TravelModeService
{
    public interface ITravelModeService
    {
        public Task<IEnumerable<TravelMode>> GetTravelModeAsync();

        public Task SetTravelModeAsync(TravelMode travelMode);

        public Task<string> GetTravelModeByIdAsync(int id);

    }
}
