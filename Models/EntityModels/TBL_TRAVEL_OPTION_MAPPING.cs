using System.ComponentModel.DataAnnotations;

namespace XtramileBackend.Models.EntityModels
{
    public class TBL_TRAVEL_OPTION_MAPPING
    {
        [Key]
        public int RequestId { get; set; }
    
        public int EmpId { get; set; }

        public int OptionId { get; set; }


    }
}
