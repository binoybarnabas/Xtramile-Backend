using System.ComponentModel.DataAnnotations;

namespace XtramileBackend.Models.EntityModels
{
	public class TBL_REQ_APPROVE
	{
		[Key]
		public int StatusApprovalId { get; set; }
		public int RequestId { get; set; }
		public int EmpId { get; set; }
		public int PrimaryStatusId { get; set; }
		public DateTime date { get; set; }
		public int? SecondaryStatusId { get; set; }
    }
}