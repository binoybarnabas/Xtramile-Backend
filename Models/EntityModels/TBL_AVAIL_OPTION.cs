using System.ComponentModel.DataAnnotations;

namespace XtramileBackend.Models.EntityModels
{
    public class TBL_AVAIL_OPTION
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