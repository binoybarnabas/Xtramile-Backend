using System.ComponentModel.DataAnnotations;

namespace XtramileBackend.Models.EntityModels
{
    public class TBL_REQUEST
    {
        [Key]
        public int RequestId { get; set; }

        public string? RequestCode { get; set; }

        public int ProjectId { get; set; }

        public string TripType { get; set; }

        public int TravelModeId { get; set; }


        //Trip Informations
        public int TravelTypeId { get; set; }

        public string TripPurpose { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime? ReturnDate { get; set; }


        public string SourceCity { get; set; }

        public string DestinationCity { get; set; }


        public string SourceCountry { get; set; }

        public string DestinationCountry { get; set; }



        //Additional Informations
        public string CabRequired { get; set; }

        public string? PrefPickUpTime { get; set; }

        public string AccommodationRequired { get; set; }

        public string PrefDepartureTime { get; set; }

        public string? AdditionalComments { get; set; }

        public int? PriorityId { get; set; }

        public int? PerdiemId { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int? ReasonId { get; set; }
        
        public int ProjectId { get; set; }

        public string TripType { get; set; }

        public int TravelModeId { get; set; }

        public string PrefPickUpTime { get; set; }


        //remove on code optimizations

        public string? DestinationCityZipCode { get; set; }
        public string? SourceCityZipCode { get; set; }
        public string? SourceState { get; set; }
        public string? DestinationState { get; set; }


    }
}
