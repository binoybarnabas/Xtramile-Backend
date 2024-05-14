namespace XtramileBackend.Models.APIModels
{
    public class EmployeeViewReq
    {
       public int RequestId { get; set; }
       public string ProjectCode { get; set; }
       public string From { get; set; }
       public string To { get; set; }
       public DateTime RequestedOn { get; set; }
       public DateTime ClosedOn { get; set;}
       public string Status { get; set; }
       public string RequestCode {  get; set; } 
    }
}