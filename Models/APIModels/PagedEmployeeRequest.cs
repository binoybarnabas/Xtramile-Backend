namespace XtramileBackend.Models.APIModels
{
    // Dto to display the paged result of employee requests
    public class PagedEmployeeRequestDto
    {
        public IReadOnlyCollection<EmployeeRequestDto> EmployeeRequest { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
