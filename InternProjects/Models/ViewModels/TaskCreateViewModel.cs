using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternProjects.Models.ViewModels
{
    public class TaskCreateViewModel
    {
        [Required(ErrorMessage = "Заглавието е задължително")]
        [StringLength(200)]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Избери категория")]
        [Display(Name = "Категория")]
        public int? CategoryId { get; set; }

        [StringLength(300)]
        [Display(Name = "Кратко описание")]
        public string? ShortDescription { get; set; }

        [Display(Name = "Цялостна характеристика")]
        public string? LongDescription { get; set; }

        [Required(ErrorMessage = "Форматът на предаване е задължителен")]  
        [Display(Name = "Формат на предаване")]
        public string SubmissionFormat { get; set; } = "";

        [Required(ErrorMessage = "Зададените часове са задължителни")]      
        [Range(0.5, 200, ErrorMessage = "Часовете трябва да са между 0.5 и 200")]
        [Display(Name = "Зададени часове")]
        public float? AssignedHours { get; set; }

        [Required(ErrorMessage = "Избери сложност")]
        public string? Difficulty { get; set; }

        [Required(ErrorMessage = "Избери приоритет")]
        public string? Priority { get; set; }

        [Display(Name = "Подходяща за (специалности)")]
        public string? SuitableFor { get; set; }

        [Display(Name = "Екипна задача")]
        public bool IsTeamTask { get; set; }

        [Range(1, 10)]
        [Display(Name = "Максимален брой стажанти")]
        public int MaxInterns { get; set; } = 1;

        [Display(Name = "Срок")]
        [DataType(DataType.Date)]
        public DateTime? Deadline { get; set; }

        [Display(Name = "Публикувай веднага")]
        public bool PublishNow { get; set; }   

        public List<SelectListItem> Categories { get; set; } = new();
    }
    public class TaskEditViewModel : TaskCreateViewModel
    {
        public int Id { get; set; }
        public string Status { get; set; } = "";
    }
}