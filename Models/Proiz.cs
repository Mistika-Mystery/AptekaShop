using System.ComponentModel.DataAnnotations;

namespace apteka.Models
{
    public class Proiz
    {
        [Key]
        public int IDPr { get; set; }
        [Required]
        [StringLength(50)]
        public string? Naz { get; set; }
        public ICollection<Lek>? Leks   { get; set; }
    }
}
