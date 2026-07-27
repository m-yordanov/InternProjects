using InternProjects.Models;

namespace InternProjects.Models.ViewModels
{
    public class InternProfileViewModel
    {
        public Intern Intern { get; set; } = null!;
        public List<TaskAssignment> Assignments { get; set; } = new();
        public List<TimeLog> TimeLogs { get; set; } = new();

        public int ProgressPercent => Intern.TotalHours > 0
            ? (int)Math.Round(Intern.ReportedHours / Intern.TotalHours * 100)
            : 0;

        public int AcceptedCount { get; set; }
        public int InProgressCount { get; set; }
        public int ReturnedCount { get; set; }
    }
}