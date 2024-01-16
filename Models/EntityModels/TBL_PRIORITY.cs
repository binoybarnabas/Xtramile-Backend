using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtramileBackend.Models.EntityModels
{
    [Table("TBL_PRIORITY")]
    public class TBL_PRIORITY
    {
        [Key]
        public int PriorityId { get; set; }
        public string PriorityName { get; set; }
        public string? Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? Modifiedby { get; set; }
        public DateTime? ModifiedOn { get; set; }

    }
}
