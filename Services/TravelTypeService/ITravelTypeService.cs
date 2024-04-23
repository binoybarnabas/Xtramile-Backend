using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.TravelTypeService
{
    public interface ITravelTypeService
    {
        public Task<IEnumerable<TravelType>> GetTravelTypeAsync();

        public Task SetTravelTypeAsync(TravelType travelType);
    }
}
