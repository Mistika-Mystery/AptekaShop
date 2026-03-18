using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apteka.Models
{
    public class Lek
    {
        [Key]
        public int IDL { get; set; }
        [Required]
        [StringLength(50)]
        public string? Naz { get; set; }

        public int Qena { get; set; }
        public Proiz? Proiz { get; set; }
        public Akqia? Akqia { get; set; }
        public VidZ? VidZ { get; set; }
        public string? Foto { get; set; }
        public ICollection<ZakLek>? ZakLeks { get; set; }
    }
}
