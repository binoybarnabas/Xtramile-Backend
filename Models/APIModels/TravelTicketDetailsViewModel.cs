namespace XtramileBackend.Models.APIModels
{
    public class TicketDetails
    {
        public IFormFile TicketFile { get; set; }
        public string? Description { get; set; }
    }

    public class TravelTicketDetailsViewModel
    {
        public IEnumerable<TicketDetails> Tickets { get; set; }
        public string RequestId { get; set; }
        public string EmpId { get; set; }
    }
}