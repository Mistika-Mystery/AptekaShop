using System.ComponentModel.DataAnnotations;

namespace CustomIdentityApp.ViewModels
{
    public class Registr
    {
        [Required]
        [Display(Name = "ФИО")]
        public string? Name { get; set; }

        [Required]
        [Display(Name = "Год рождения")]
        public string Login { get; set; }
        public int Age { get; set; }
        [Required]
        [Display(Name = "Роль")]
        public string? Role { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }


        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string? PasswordConfirm { get; set; }
    }
}