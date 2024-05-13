    namespace XtramileBackend.Models.APIModels
{
    public class RequestTableViewTravelAdmin
    {
        public int RequestId { get; set; }
        public string ProjectCode { get; set; }
        public string EmployeeName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string TravelTypeName { get; set; }
        public string PriorityName { get; set; }
        public string StatusName { get; set; }
        public DateTime ApprovalDate { get; set; }
        public DateTime date { get; set; }
        public string RequestCode { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string TravelType { get; set; }
        public DateTime DepartureDate { get; set; }

    }
}
