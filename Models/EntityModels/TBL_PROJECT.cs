using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtramileBackend.Models.EntityModels
{
    [Table("TBL_PROJECT")]
    public class TBL_PROJECT
    {
        [Key]
        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public int DepartmentId{ get; set; }
        public string? Description{ get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public int? Modifiedby {  get; set; }
        public DateTime? ModifiedOn {  get; set; }
    }
}
