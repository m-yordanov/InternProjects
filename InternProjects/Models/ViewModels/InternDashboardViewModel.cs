using InternProjects.Models;

namespace InternProjects.Models.ViewModels
{
    public class InternDashboardViewModel
    {
        public string InternName { get; set; } = "";

        public float TotalHours { get; set; }
        public float TaskHours { get; set; }
        public float AddedHours { get; set; }
        public float ReportedHours { get; set; }
        public float RemainingHours { get; set; }
        public int ProgressPercent => TotalHours > 0
            ? (int)Math.Round(ReportedHours / TotalHours * 100)
            : 0;

        public List<TaskAssignment> ActiveTasks { get; set; } = new();
        public List<TaskAssignment> ReturnedTasks { get; set; } = new();
        public List<TaskAssignment> SubmittedTasks { get; set; } = new();
        public List<TaskAssignment> AcceptedTasks { get; set; } = new();

        public string? LatestFeedback { get; set; }
        public List<TaskItem> FreeTasks { get; set; } = new();
    }
}
