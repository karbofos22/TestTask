using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestTask.Models
{
    public class Offer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? VendorCode { get; set; } 
        public string? Description { get; set; } 
        public int? Price { get; set; }
        public bool IsAlreadyInDB { get; set; }
    }
}
