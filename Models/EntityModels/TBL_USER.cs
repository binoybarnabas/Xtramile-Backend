using System.ComponentModel.DataAnnotations;

namespace XtramileBackend.Models.EntityModels
{
    public class TBL_USER
    {
        [Key]
        public int EmpId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
