namespace XtramileBackend.Models.APIModels
{
    public class DashboardUpcomingTrip
    {
        public string TripPurpose { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string SourceCity { get; set; }
        public string? SourceState { get; set; }
        public string SourceCountry { get; set; }
        public string DestinationCity { get; set; }
        public string? DestinationState { get; set; }
        public string DestinationCountry { get; set; }
    }
}
