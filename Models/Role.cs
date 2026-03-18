using System.ComponentModel.DataAnnotations;

namespace apteka.Models
{
    public class Role
    {
        [Key]
        public int idRole { get; set; }
        [Required]
        [StringLength(50)]
        public string? name { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}

