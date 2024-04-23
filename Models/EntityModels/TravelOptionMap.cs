using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtramileBackend.Models.EntityModels
{
    [Table("TBL_TRAVEL_OPTION_MAPPING")]
    public class TravelOptionMap
    {
        [Key]
        public int RequestId { get; set; }
    
        public int EmpId { get; set; }

        public int OptionId { get; set; }


    }
}
