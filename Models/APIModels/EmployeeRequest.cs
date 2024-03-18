

namespace XtramileBackend.Models.APIModels
{
	public class EmployeeRequestDto
	{
		public int? RequestId { get; set; }
		public string? EmployeeName { get; set; }

		public string? Email { get; set; }

		public string? ProjectCode { get; set; }
		
		public DateTime? Date { get; set; }
		public string? Mode {  get; set; }

		public string? Status { get; set; }

		public DateTime StatusDate { get; set; }
    }

}