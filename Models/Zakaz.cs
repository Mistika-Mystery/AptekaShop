using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apteka.Models
{
    public class Zakaz
    {
        [Key]
        public int IDZ { get; set; }
        [Required]
        [StringLength(50)]

        public DateTime Data { get; set; }
   
        public User? Users { get; set; }

        public ICollection<ZakLek>? ZakLeks { get; set; }
    }
}
