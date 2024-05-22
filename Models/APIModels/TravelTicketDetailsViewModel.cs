namespace XtramileBackend.Models.APIModels
{
    public class TravelTicketDetailsViewModel
    {
        //public IFormFile? TravelAuthorizationEmailCapture { get; set; }

        public required string RequestId { get; set; }

        public required string EmpId { get; set; }

        public required IFormFile[] Tickets { get; set; }

        public string[]? Description { get; set; }


    }
}


