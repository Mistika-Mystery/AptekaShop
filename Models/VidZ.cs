using System.ComponentModel.DataAnnotations;

namespace apteka.Models
{
    public class VidZ
    {
        [Key]
        public int IDV { get; set; }
        [Required]
        [StringLength(50)]
        public string? Naz { get; set; }
        public ICollection<Lek>? Leks { get; set; }
    }
}
