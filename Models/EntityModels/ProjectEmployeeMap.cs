using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtramileBackend.Models.EntityModels
{
	[Table("TBL_PROJECT_MAPPING")]
	public class ProjectEmployeeMap
	{
		[Key]
		public int EmpProjectId { get; set; }
		public int EmpId { get; set; }
		public int ProjectId { get; set; }
	}
}