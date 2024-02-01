namespace XtramileBackend.Models.APIModels
{
    public class PendingRequetsViewEmployee
    {
        public string modifiedBy { get; set; }
        public string statusName { get; set; }
        public int requestId { get; set; }
        public string projectName { get; set; }
        public string reasonOfTravel { get; set; }
        public DateTime dateOfTravel { get; set; }
        
    }
}
