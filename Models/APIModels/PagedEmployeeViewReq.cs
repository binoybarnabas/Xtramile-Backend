namespace XtramileBackend.Models.APIModels
{
    public class PagedEmployeeViewReqDto
    {
        public IReadOnlyCollection<EmployeeViewReq> EmployeeRequest { get; set; }
        public int TotatlCount { get; set; }
        public int TotalPages { get; set; }
    }

}
