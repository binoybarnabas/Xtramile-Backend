namespace XtramileBackend.Models.APIModels
{
    public class EmployeeCurrentRequest
    {
        public DateTime DepartureDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
