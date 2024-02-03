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
    }
}
