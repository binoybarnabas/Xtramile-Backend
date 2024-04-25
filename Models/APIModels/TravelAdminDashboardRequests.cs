using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Models.APIModels
{
    public class TravelAdminDashboardRequests
    {
        //using ClosedTravelAdmin as it already has RequestId, Name, From, To and Status 
        public IEnumerable<ClosedTravelAdmin> IncomingRequests { get; set; }
        public IEnumerable<ClosedTravelAdmin> WaitingSelectedRequests { get; set; }
        public IEnumerable<ClosedTravelAdmin> OngoingRequests { get; set; }
        public IEnumerable<ClosedTravelAdmin> ClosedRequests { get; set; }
        public IEnumerable<Notifications> Notifications { get; set; }
    }
}
