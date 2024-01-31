namespace XtramileBackend.Models.APIModels
{
    public class EmployeeInfo
    {
        public int EmpId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string DepartmentName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string? ReportsTo { get; set; }
    }
}
