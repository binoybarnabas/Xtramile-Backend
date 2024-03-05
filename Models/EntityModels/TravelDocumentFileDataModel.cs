using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtramileBackend.Models.EntityModels
{
    [Table("TBL_TRAVEL_DOC_FILE_DATA")]
    public class TravelDocumentFileDataModel
    {
        [Key]
        public int TravelDocFileId { get; set; }
        public int TravelDocTypeId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int FileTypeId { get; set; }
        public int UploadedBy { get; set; }
        public int? CountryId { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
