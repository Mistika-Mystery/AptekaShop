using System.ComponentModel.DataAnnotations;

namespace CustomIdentityApp.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "ФИО")]
        public string? FIO { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}