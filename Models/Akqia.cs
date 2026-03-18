using apteka.Models;
using System.ComponentModel.DataAnnotations;

namespace apteka.Models
{
    public class Akqia
    {
        [Key]
        public int IDAk { get; set; }
        [Required]
        [StringLength(50)]
        public string? Naz { get; set; }
        public int Pr {get; set;}
        public ICollection<Lek>? Leks { get; set; }
    }
}
