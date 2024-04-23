using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtramileBackend.Models.EntityModels
{
    [Table("TBL_USER")]
    public class User
    {
        [Key]
        public int EmpId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
