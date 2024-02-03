using XtramileBackend.Models.APIModels;

namespace XtramileBackend.Services.TravelAdminService
{
    public interface ITravelAdminService
    {
        public Task<IEnumerable<OngoingTravelAdmin>> OnGoingTravel();
        public Task<IEnumerable<RequestTableViewTravelAdmin>> GetIncomingRequests();
        public Task<OptionCard> GetSelectedOptionFromEmployee(int reqId);
        public Task<IEnumerable<RequestTableViewTravelAdmin>> GetTravelRequests(string statusCode);
    }
}
