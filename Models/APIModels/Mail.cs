namespace XtramileBackend.Models.APIModels
{
    public class Mail
    {
        public string recipientEmail { get; set; }
        public string recipientName { get; set; }
        public string? managerName { get; set; }
        public string? requestSubmittedBy { get; set; }
        public string emailBody { get; set; }
    }
}
