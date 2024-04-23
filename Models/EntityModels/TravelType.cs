using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtramileBackend.Models.EntityModels
{
    [Table("TBL_TRAVEL_TYPE")]
    public class TravelType
    {
        [Key]
        public int TravelTypeID { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
