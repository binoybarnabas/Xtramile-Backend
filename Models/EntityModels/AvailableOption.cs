using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtramileBackend.Models.EntityModels
{
    [Table("TBL_AVAIL_OPTION")]
    public class AvailableOption
    {
        [Key]
        public int OptionId { get; set; }
        public string? Description { get; set; }
        public string Class { get; set; }
        public string ServiceOfferedBy { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int RequestId { get; set; }
        public int CategoryId { get; set; }
        public int? ModeId { get; set; }
    }
}