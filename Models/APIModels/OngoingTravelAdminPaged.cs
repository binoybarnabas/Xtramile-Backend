using System.Collections.ObjectModel;

namespace XtramileBackend.Models.APIModels
{
    public class OngoingTravelAdminPaged
    {
        public IReadOnlyCollection<OngoingTravelAdmin>? OngoingTravel { get; set;}
        public int TotalCount { get; set;}  

    }
}
