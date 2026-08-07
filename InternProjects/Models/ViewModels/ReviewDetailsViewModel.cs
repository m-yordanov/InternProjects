namespace InternProjects.Models.ViewModels
{
    public class ReviewDetailsViewModel
    {
        public TaskAssignment Assignment { get; set; } = null!;
        public List<Submission> Submissions { get; set; } = new();
        public Submission? LatestSubmission { get; set; }
        public bool IsTeamTask { get; set; }
        public List<TaskAssignment> TeamMembers { get; set; } = new();
    }

    public class ReviewListItemViewModel
    {
        public int AssignmentId { get; set; }
        public string TaskTitle { get; set; } = "";
        public string? CategoryName { get; set; }
        public float AssignedHours { get; set; }
        public bool IsTeamTask { get; set; }
        public DateTime? SubmitDate { get; set; }
        public List<string> MemberNames { get; set; } = new();
    }
}
