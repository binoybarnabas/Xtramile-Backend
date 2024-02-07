using XtramileBackend.Models.APIModels;

namespace XtramileBackend.Services.TravelAdminService
{
    public interface ITravelAdminService
    {
        public Task<IEnumerable<OngoingTravelAdmin>> OnGoingTravel();
        public Task<RequestTableViewTravelAdminPaged> GetIncomingRequests(int pageIndex, int pageSize);
        public Task<OptionCard> GetSelectedOptionFromEmployee(int reqId);
        public Task<IEnumerable<RequestTableViewTravelAdmin>> GetTravelRequests(string statusCode);

        public Task<TravelRequestEmployeeViewModel> GetEmployeeRequestDetail(int requestId);

        public Task<RequestTableViewTravelAdminPaged> GetIncomingRequestsSorted(int pageIndex, int pageSize, bool priority, bool status, bool travelType);

    }
}
