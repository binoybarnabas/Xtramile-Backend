using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtramileBackend.Models.EntityModels
{
    [Table("TBL_FILE_TYPE")]
    public class TBL_FILE_TYPE
    {
        [Key]
        public int FileTypeId { get; set; }
        public string FileType { get; set; }
        public string? Description { get; set; }
        public string? FileExtension { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? Modifiedby { get; set; }
        public DateTime? ModifiedOn { get; set; }

    }
}
