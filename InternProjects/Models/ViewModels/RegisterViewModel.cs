using System.ComponentModel.DataAnnotations;

namespace InternProjects.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Името е задължително")]
        [Display(Name = "Име")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Фамилията е задължителна")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = "";

        [Required(ErrorMessage = "Имейлът е задължителен")]
        [EmailAddress(ErrorMessage = "Невалиден имейл")]
        [Display(Name = "Имейл")]
        public string Email { get; set; } = "";

        [Display(Name = "Телефон")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Университет/Училище")]
        public string? University { get; set; }

        [Display(Name = "Специалност")]
        public string? Specialty { get; set; }

        [Required(ErrorMessage = "Паролата е задължителна")]
        [MinLength(8, ErrorMessage = "Минимум 8 символа")]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Моля потвърдете паролата")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Паролите не съвпадат")]
        [Display(Name = "Потвърди паролата")]
        public string ConfirmPassword { get; set; } = "";
    }
}
