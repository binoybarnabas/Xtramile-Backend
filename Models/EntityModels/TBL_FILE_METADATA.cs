using System.ComponentModel.DataAnnotations;

namespace XtramileBackend.Models.EntityModels
{
    public class TBL_FILE_METADATA
    {
        [Key]
        public int FileId { get; set; }

        public int? RequestId { get; set; }

        public string FileName { get; set; }
        
        public string FilePath { get; set; }

        public string Description { get; set; }

        public int FileTypeId { get; set; }

        public int CreatedBy { get; set; }
        
        public DateTime CreatedOn { get; set;}

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }


    }
}
