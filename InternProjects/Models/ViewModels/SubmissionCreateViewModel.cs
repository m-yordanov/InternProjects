using System.ComponentModel.DataAnnotations;

namespace InternProjects.Models.ViewModels
{
    public class SubmissionCreateViewModel
    {
        public int AssignmentId { get; set; }

        public string TaskTitle { get; set; } = "";
        public string? SubmissionFormat { get; set; }

        public string? SubmittedText { get; set; }

        [Url(ErrorMessage = "Невалиден линк")]
        public string? SubmittedLink { get; set; }

        public IFormFile? File { get; set; }
        public List<IFormFile>? Photos { get; set; }

        public string? SubmittedNotes { get; set; }
    }
}