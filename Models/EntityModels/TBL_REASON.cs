using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtramileBackend.Models.EntityModels
{
    [Table("TBL_REASON")]
    public class TBL_REASON
    {
        [Key]
        public int ReasonId { get; set; }
        public string ReasonCode { get; set; }
        public string? Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? Modifiedby { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
