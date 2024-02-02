namespace XtramileBackend.Models.APIModels
{
    public class EmployeeViewReq
    {
       public int RequestId { get; set; }
       public string ProjectCode { get; set; }
       public string ProjectName { get; set; }
       public string TravelType { get; set; }
       public DateOnly ClosedDate { get; set; }
       public string Status { get; set;}
    }
}