namespace XtramileBackend.Models.APIModels
{
    public class Mail
    {
        public string recipientEmail { get; set; }
        public string recipientName { get; set; }
        public string mailContext { get; set; }
        public string requestCode { get; set; }
        public string? managerName { get; set; }
        public string? reasonForRejection {get; set;}
    }
}
