namespace XtramileBackend.Models.APIModels
{
    public class EmployeeOngoingRequest
    {
        public int RequestId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public string StatusName { get; set; }
    }
}
