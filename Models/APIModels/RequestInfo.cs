using System.ComponentModel.DataAnnotations;

namespace XtramileBackend.Models.APIModels
{
    public class RequestInfo
    {
        
        public int RequestId { get; set; }

        public string? RequestCode { get; set; }


        //Trip Informations
        public int TravelTypeId { get; set; }

        public string TripPurpose { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public string? SourceCityZipCode { get; set; }

        public string? DestinationCityZipCode { get; set; }


        public string SourceCity { get; set; }


        public string DestinationCity { get; set; }

        public string SourceState { get; set; }

        public string? DestinationState { get; set; }

        public string SourceCountry { get; set; }

        public string DestinationCountry { get; set; }



        //Additional Informations
        public string CabRequired { get; set; }

        public string AccommodationRequired { get; set; }

        public string PrefDepartureTime { get; set; }

        public string AdditionalComments { get; set; }

        public int? PriorityId { get; set; }

        public int? PerdiemId { get; set; }

        public int CreatedBy { get; set; }

    }
}
