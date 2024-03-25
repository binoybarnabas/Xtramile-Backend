namespace XtramileBackend.Models.APIModels
{
    public class TravelDocumentViewModel
    {
        public string UploadedBy { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime UploadedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Country { get; set; }
        public string DocumentType { get; set; }
        public string DocumentSize { get; set; }
        public string DocumentURL { get; set; }
        public int? RemainingDays { get; set; }
    }
}
