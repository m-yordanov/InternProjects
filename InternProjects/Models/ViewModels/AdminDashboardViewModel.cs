using InternProjects.Models;

namespace InternProjects.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalInterns { get; set; }
        public int ActiveInterns { get; set; }

        public int TasksForReview { get; set; }
        public int FreeTasks { get; set; }
        public int TasksInProgress { get; set; }
        public int ReturnedTasks { get; set; }
        public int AcceptedTasks { get; set; }

        public float TotalReportedHours { get; set; }

        public List<Intern> LowActivityInterns { get; set; } = new();
        public List<Intern> NearCompletionInterns { get; set; } = new();
        public List<TaskAssignment> LatestSubmissions { get; set; } = new();
    }
}
