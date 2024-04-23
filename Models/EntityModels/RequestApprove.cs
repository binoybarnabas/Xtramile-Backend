using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtramileBackend.Models.EntityModels
{
	[Table("TBL_REQ_APPROVE")]
	public class RequestApprove
	{
		[Key]
		public int StatusApprovalId { get; set; }
		public int RequestId { get; set; }
		public int EmpId { get; set; }
		public int PrimaryStatusId { get; set; }
		public DateTime date { get; set; }
		public int SecondaryStatusId { get; set; }
    }
}