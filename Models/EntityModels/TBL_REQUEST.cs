using System.ComponentModel.DataAnnotations;

namespace XtramileBackend.Models.EntityModels
{
    public class TBL_REQUEST
    {
        [Key]
        public int RequestId { get; set; }
        public string RequestCode { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int TravelTypeId { get; set; }     
        public int? PerdiumId { get; set; }
        public int ProjectId { get; set; }
        public int ReasonId { get; set; }
        public int? PriorityId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string SourceCity { get; set; }
        public string? SourceState { get; set; }
        public string SourceCountry { get; set; }
        public string DestinationCity { get; set; }
        public string? DestinationState { get; set; }
        public string DestinationCountry { get; set; }  
        public string CabRequired { get; set; }
        public string AccommodationRequired { get; set; }
    }
}
