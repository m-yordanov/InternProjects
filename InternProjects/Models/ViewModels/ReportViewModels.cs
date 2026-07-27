using InternProjects.Models;

namespace InternProjects.Models.ViewModels
{
    public class TaskReportRow
    {
        public string Title { get; set; } = "";
        public string Category { get; set; } = "";
        public string Status { get; set; } = "";
        public string ChosenBy { get; set; } = "";
        public float AssignedHours { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? AcceptDate { get; set; }
    }

    public class HoursReportRow
    {
        public string InternName { get; set; } = "";
        public DateTime Date { get; set; }
        public string Source { get; set; } = "";
        public string Description { get; set; } = "";
        public float Hours { get; set; }
        public string AddedBy { get; set; } = "";
    }

    public class SummaryReportRow
    {
        public int InternId { get; set; }
        public string InternName { get; set; } = "";
        public float TotalHours { get; set; }
        public float ReportedHours { get; set; }
        public float RemainingHours { get; set; }
        public int Percent => TotalHours > 0
            ? (int)Math.Round(ReportedHours / TotalHours * 100) : 0;
    }
}