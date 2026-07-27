namespace InternProjects.Models.ViewModels
{
    public class ReviewDetailsViewModel
    {
        public TaskAssignment Assignment { get; set; } = null!;
        public List<Submission> Submissions { get; set; } = new();
        public Submission? LatestSubmission { get; set; }
    }
}
