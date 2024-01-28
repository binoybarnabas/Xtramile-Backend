namespace XtramileBackend.Models.APIModels
{
    public class InvoiceAttachment
    {
        public int InvoiceId { get; set; }
        public string VendorName { get; set; }
        public string VendorEmail {  get; set; }
        public double? Amount { get; set; }
        public DateTime? PaidDate { get; set; }
        public string status { get; set; }

    }
}
