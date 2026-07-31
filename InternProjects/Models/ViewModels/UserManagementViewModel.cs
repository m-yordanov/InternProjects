using System.ComponentModel.DataAnnotations;

namespace InternProjects.Models.ViewModels
{
    public class UserListItemViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
        public string Status { get; set; } = "";
        public DateTime? LastLogin { get; set; }
        public int? InternId { get; set; }
    }

    public class UserCreateViewModel
    {
        [Required(ErrorMessage = "Името е задължително")]
        [Display(Name = "Име")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Фамилията е задължителна")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = "";

        [Required(ErrorMessage = "Имейлът е задължителен")]
        [EmailAddress(ErrorMessage = "Невалиден имейл")]
        public string Email { get; set; } = "";

        [Display(Name = "Телефон")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Паролата е задължителна")]
        [MinLength(8, ErrorMessage = "Минимум 8 символа")]
        [DataType(DataType.Password)]
        [Display(Name = "Първоначална парола")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Избери роля")]
        public string Role { get; set; } = "Intern";

        [Display(Name = "Университет/Училище")]
        public string? University { get; set; }

        [Display(Name = "Специалност")]
        public string? Specialty { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Начало на стажа")]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        [Display(Name = "Край на стажа")]
        public DateTime? EndDate { get; set; }

        [Range(1, 1000)]
        [Display(Name = "Общо изискуеми часове")]
        public float TotalHours { get; set; } = 240;

        [Display(Name = "Бележки")]
        public string? Notes { get; set; }
    }

    public class UserEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името е задължително")]
        [Display(Name = "Име")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Фамилията е задължителна")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = "";

        [Required, EmailAddress(ErrorMessage = "Невалиден имейл")]
        public string Email { get; set; } = "";

        [Display(Name = "Телефон")]
        public string? PhoneNumber { get; set; }


        [MinLength(8, ErrorMessage = "Минимум 8 символа")]
        [DataType(DataType.Password)]
        [Display(Name = "Нова парола (остави празно, за да не се променя)")]
        public string? NewPassword { get; set; }


        public bool IsIntern { get; set; }


        [Display(Name = "Университет/Училище")]
        public string? University { get; set; }


        [Display(Name = "Специалност")]
        public string? Specialty { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Начало на стажа")]
        public DateTime StartDate { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Край на стажа")]
        public DateTime? EndDate { get; set; }


        [Range(1, 1000)]
        [Display(Name = "Общо изискуеми часове")]
        public float TotalHours { get; set; }


        [Display(Name = "Бележки")]
        public string? Notes { get; set; }
    }
}