using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtramileBackend.Models.EntityModels
{
    [Table("TBL_NOTIFICATION")]
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }
        public int EmployeeId { get; set; }
        public int RequestId { get; set; }
        public string NotificationBody { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
