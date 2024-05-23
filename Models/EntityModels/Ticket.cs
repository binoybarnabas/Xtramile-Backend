using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtramileBackend.Models.EntityModels
{
    [Table("TBL_TICKET")]
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        public int FileId { get; set; }
        public string? Description { get; set; }
    }
}
