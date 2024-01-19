using System.ComponentModel.DataAnnotations;

namespace XtramileBackend.Models.EntityModels
{
    public class TBL_AVAIL_OPTION
    {
        [Key]
        public int OptionId { get; set; }
        public string OptionCode { get; set; }
        public string? Description { get; set; }
        public string OptionInfo { get; set; }
        public string ServiceOffered { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int RequestId { get; set; }
        public int CategoryId { get; set; }
    }
}