namespace XtramileBackend.Models.APIModels
{
    public class TravelRequestEmployeeViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string? ReportsTo { get; set; }
        public string DepartmentName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }

        // Trip Information Section
        public string TravelType { get; set; }
        public string TripPurpose { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ReturnDate { get; set; }
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
        public IFormFile? TravelAuthorizationEmailCapture { get; set; }
        public IFormFile? PassportAttachment { get; set; }
        public IFormFile? IdCardAttachment { get; set; }
        public string? AdditionalComments { get; set; }
    }
}
