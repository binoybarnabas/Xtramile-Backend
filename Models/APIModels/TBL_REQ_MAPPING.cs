using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtramileBackend.Models.APIModels
{
    [Table("TBL_REQ_MAPPING")]
    public class TBL_REQ_MAPPING
    {
        [Key]
        public int RequestId { get; set; }
        public int EmpId { get; set; }
        public int OptionId { get; set; }

    }
}
