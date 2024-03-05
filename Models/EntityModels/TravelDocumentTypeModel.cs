using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtramileBackend.Models.EntityModels
{
    [Table("TBL_TRAVEL_DOC_TYPE")]
    public class TravelDocumentTypeModel
    {
        [Key]
        public int TravelDocumentTypeId { get; set; }
        public string TravelDocumentTypeName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
