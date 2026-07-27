using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternProjects.Models.ViewModels
{
    public class ManualHoursViewModel
    {
        [Required(ErrorMessage = "Избери стажант")]
        [Display(Name = "Стажант")]
        public int? InternId { get; set; }

        [Required(ErrorMessage = "Датата е задължителна")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата на дейността")]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Часовете са задължителни")]
        [Range(0.5, 12, ErrorMessage = "Между 0.5 и 12 часа на запис")]
        [Display(Name = "Брой часове")]
        public float? Hours { get; set; }

        [Required(ErrorMessage = "Описанието е задължително (т. 23.5)")]
        [StringLength(500)]
        [Display(Name = "Описание на дейността")]
        public string Description { get; set; } = "";

        [StringLength(500)]
        [Display(Name = "Бележка")]
        public string? Note { get; set; }

        public List<SelectListItem> Interns { get; set; } = new();
    }
}