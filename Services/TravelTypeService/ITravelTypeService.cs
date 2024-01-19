using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.TravelTypeService
{
    public interface ITravelTypeService
    {
        public Task<IEnumerable<TBL_TRAVEL_TYPE>> GetTravelTypeAsync();

        public Task SetTravelTypeAsync(TBL_TRAVEL_TYPE travelType);
    }
}
