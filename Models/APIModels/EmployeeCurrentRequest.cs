namespace XtramileBackend.Models.APIModels
{
    public class EmployeeCurrentRequest
    {
        public DateTime DepartureDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? DepartureTime { get; set; }
        public string? Source { get; set; }
        public string? Destination { get; set; }
        public string? Purpose { get; set; }
       
    }
}
