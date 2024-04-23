using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtramileBackend.Models.EntityModels
{
    [Table("TBL_TRAVEL_OPTION")]
    public class TravelOption
    {
        [Key]
        public int OptionId { get; set; }

        public int RequestId { get; set; }

        public string? Description { get; set; }

        public int? FileId { get; set; }


    }
}
