using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apteka.Models
{
    public class ZakLek
    {
        [Key]
        public int IDZL { get; set; }
        [Required]

        public Zakaz? Zakaz { get; set; }
        public Lek? Lek { get; set; }
    }
}
