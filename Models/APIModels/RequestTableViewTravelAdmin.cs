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
        public DateTime ApprovalDate { get; internal set; }
    }
}
