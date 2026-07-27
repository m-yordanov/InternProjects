using System.ComponentModel.DataAnnotations;

namespace InternProjects.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Моля въведете имейл")]
        [EmailAddress(ErrorMessage = "Невалиден имейл")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Моля въведете парола")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        public bool RememberMe { get; set; }
    }
}
