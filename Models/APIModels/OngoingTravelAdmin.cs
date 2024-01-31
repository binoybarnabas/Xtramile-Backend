namespace XtramileBackend.Models.APIModels
{
    public class OngoingTravelAdmin
    {
        public string RequestCode{ get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SourceCity { get; set; }
        public string DestinationCity { get; set; }
    }
}
