namespace XtramileBackend.Models.APIModels
{
    public class TravelDocument
    {
        public string Country { get; set; }
        public DateTime ExpiryDate { get; set; }
        public IFormFile File { get; set; }
        public string TravelDocType { get; set; }
        public int UploadedBy { get; set; }
    }
}