using System.ComponentModel.DataAnnotations;

namespace XtramileBackend.Models.EntityModels
{
    public class TBL_TRAVEL_OPTION
    {
        [Key]
        public int OptionId { get; set; }

        public int RequestId { get; set; }

        public string? Description { get; set; }

        public int? FileId { get; set; }


    }
}
