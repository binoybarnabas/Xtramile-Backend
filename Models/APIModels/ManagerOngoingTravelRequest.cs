namespace XtramileBackend.Models.APIModels
{
    public class ManagerOngoingTravelRequest
    {
        public int RequestId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string ProjectCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public string TravelTypeName { get; set; }
        public string PriorityName { get; set; }
        public string StatusName { get; set; }
        public string RequestCode { get; set; }
        public DateTime date { get; set; }
        public string TicketStatus { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime DepartureDate { get; set; }
    }
}
