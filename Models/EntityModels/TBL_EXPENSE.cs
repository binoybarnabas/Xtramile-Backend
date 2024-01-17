using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace XtramileBackend.Models.EntityModels
{
    [Table("TBL_EXPENSE")]
    public class TBL_EXPENSE
    {
        [Key]
        public int UtrId { get; set; }
        public int? InvoiceId { get; set; }
        public string? Description { get; set; }
        public string VendorName { get; set; }
        public string? VendorContact { get; set; }
        public string? VendorEmail { get; set; }
        public double InvoiceAmount { get; set; }

    }
}
