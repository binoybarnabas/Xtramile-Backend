using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace XtramileBackend.Models.EntityModels
{

    //   [Table("TBL_COUNTRY")]

    public class TEST_ITEM
    {


        public int CreatedBy { get; set; }

        public int TravelTypeId { get; set; }

        public string TripPurpose { get; set; }

        public string? SourceCityZipCode { get; set; }

        public string? DestinationCityZipCode { get; set; }

        public string SourceCountry { get; set; }

        public string DestinationCountry { get; set; }

        public string SourceCity { get; set; }

        public string DestinationCity { get; set; }

        public string SourceState { get; set; }

        public string? DestinationState { get; set; }

        public string CabRequired { get; set; }

        public string AccomodationRequired { get; set; }

        public string PrefDepartureTime { get; set; }

        public int? PriorityId { get; set; }

        public int? PerdiemId { get; set; }

        List<IFormFile> files { get; set; }

    }
}


