namespace XtramileBackend.Models.APIModels
{

    public class TravelRequestViewModel
    {
        public string CreatedBy { get; set; }

        // Trip Information Section
        public string TravelTypeId { get; set; }
        public string TripPurpose { get; set; }
        public string DepartureDate { get; set; }
        public string ReturnDate { get; set; }
        public string? SourceCityZipCode { get; set; }

        public string? DestinationCityZipCode { get; set; }

        public string SourceCity { get; set; }
        public string DestinationCity { get; set; }
        public string? SourceState { get; set; }
        public string? DestinationState { get; set; }

        public string SourceCountry { get; set; }

        public string DestinationCountry { get; set; }

        // Additional Information Section
        public string CabRequired { get; set; }
        public string AccommodationRequired { get; set; }
        public string PrefDepartureTime { get; set; }
        public IFormFile? MailAttachment { get; set; }
        public IFormFile? PassportAttachment { get; set; }
        public IFormFile? IdCardAttachment { get; set; }
        public string? AdditionalComments { get; set; }
    }
}
