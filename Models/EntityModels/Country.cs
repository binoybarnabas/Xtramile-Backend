using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XtramileBackend.Models.EntityModels
{
    [Table("TBL_COUNTRY")]
    public class Country
    {
        [Key]
        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string? Description { get; set; }
    }
}
