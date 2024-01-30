namespace XtramileBackend.Models.APIModels
{
    public class OptionCard
    {
        public int OptionId { get; set; }
        public string Class { get; set; }
        public string ServiceOfferedBy { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int RequestId { get; set; }
        public string SourceCity { get; set; }
        public string? SourceState { get; set; }
        public string SourceCountry { get; set; }
        public string DestinationCity { get; set; }
        public string? DestinationState { get; set; }
        public string DestinationCountry { get; set; }
        public int? ModeId { get; set; }
        public string ModeName { get; set; }
        public int TravelTypeId { get; set; }
        public string TravelTypeName { get; set; }
        public string DestinationCountryCode { get; set; }
        public string SourceCountryCode { get; set; }
    }
}
