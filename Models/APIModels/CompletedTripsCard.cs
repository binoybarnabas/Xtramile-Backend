namespace XtramileBackend.Models.APIModels
{
    public class CompletedTripsCard
    {
        public string SourceCity { get; set; }
        public string DestinationCity { get; set; }
        public string SourceCountryCode { get; set; }
        public string DestinationCountryCode { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string ModeName { get; set; }
        public string ProjectCode { get; set; }
        public string TripPurpose { get; set; }
        public string RequestCode { get; set; }
        
    }
}
