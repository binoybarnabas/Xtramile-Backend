namespace XtramileBackend.Models.APIModels
{
    public class UpdateRequest
    {
        public int RequestId { get; set; }
        public int EmpId { get; set; }
        public string? Description { get; set; }
        public string Action { get; set; }
    }
}
