using System.Globalization;

namespace XtramileBackend.Models.APIModels
{

    public class TravelRequestViewModel
    {

        //Additional Fields For Returns
        public string? RequestCode { get; set; }

        public string? ProjectCode {  get; set; }  

        public string? EmployeeName { get; set; }

        public string? PrimaryStatus { get; set; }

        public string? PassportFileUrl { get; set; }

        public string? TravelAuthMailFileUrl { get; set; }

        public string CreatedBy { get; set; }

        public string ProjectId { get; set; }

        // Trip Information Section

        //one way / roundtrip
        public string TripType { get; set; }

        public string TravelModeId { get; set; }

        public string TravelType { get; set; }

        public string SourceCity { get; set; }

        public string DestinationCity { get; set; }

        public string SourceCountry { get; set; }

        public string DestinationCountry { get; set; }

        public string DepartureDate { get; set; }

        public string? ReturnDate { get; set; }

        public string PrefDepartureTime { get; set; }

        public string TripPurpose { get; set; }


        // Additional Information Section
        public string CabRequired { get; set; }

        public string? PrefPickUpTime { get; set; }

        public string AccommodationRequired { get; set; }


        public IFormFile? TravelAuthorizationEmailCapture { get; set; }
        public IFormFile? PassportAttachment { get; set; }
/*        public IFormFile? IdCardAttachment { get; set; }
*/      public string? AdditionalComments { get; set; }
        public int RequestId { get; set; }
    }
}
