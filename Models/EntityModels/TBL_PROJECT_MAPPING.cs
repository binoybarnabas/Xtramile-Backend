using System.ComponentModel.DataAnnotations;

namespace XtramileBackend.Models.EntityModels
{
    public class TBL_PROJECT_MAPPING
    {
        [Key]
        public int EmpProjectId { get; set; }
        public int EmpId { get; set; }
        public int ProjectId { get; set; }
    }
}
