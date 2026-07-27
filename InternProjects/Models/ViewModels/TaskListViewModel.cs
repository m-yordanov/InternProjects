using InternProjects.Models;

namespace InternProjects.Models.ViewModels
{
    public class TaskListViewModel
    {
        public List<TaskItem> Tasks { get; set; } = new();
        public List<Category> Categories { get; set; } = new();

        public int? CategoryId { get; set; }
        public string? Difficulty { get; set; }
        public string? Search { get; set; }
    }
}