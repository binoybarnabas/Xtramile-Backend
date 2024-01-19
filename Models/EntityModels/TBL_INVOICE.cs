using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace XtramileBackend.Models.EntityModels
{
    [Table("TBL_INVOICE")]
    public class TBL_INVOICE
    {
        [Key]
        public int InvoiceId { get; set; }
        public int CategoryId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int RequestId { get; set; }
        public string? InvoiceURL { get; set; }
        public int FileTypeId { get; set; }
        public int StatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public int? Modifiedby { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
