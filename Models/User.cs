using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apteka.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        [Required]
        [StringLength(100)]
        public string? Login { get; set; }
        [Range(0, 120)]
        public int Age { get; set; }
        public string? Password { get; set; }
        [Range(0, 120)]
        public int IdRole { get; set; }
        [ForeignKey("IdRole")]
        public Role?  Role { get; set; }
        public ICollection<Zakaz>? Zakazs { get; set; }
    }
}