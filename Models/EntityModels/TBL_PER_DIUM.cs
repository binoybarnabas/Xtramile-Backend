using System.ComponentModel.DataAnnotations;

namespace XtramileBackend.Models.EntityModels
{
    public class TBL_PER_DIUM
    {
        [Key]
        public int PerdiumId { get; set; }
        public string? PerdiumName { get; set; }
        public int CountryId { get; set; }
        public string? Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
