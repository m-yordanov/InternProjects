namespace InternProjects.Models
{
    public class Submission
    {
        public int Id { get; set; }
        public string? SubmittedText { get; set; }
        public string? SubmittedLink { get; set; }
        public string? SubmittedFile { get; set; }
        public string? SubmittedPhotos { get; set; }
        public string? SubmittedNotes { get; set; }
        public DateTime SubmitDate  { get; set; }
        public int Version { get; set; }
        public string StatusSubmission { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string? MentorFeedback { get; set; }


        public int AssignmentId { get; set; }
        public TaskAssignment? Assignment { get; set; }


        public int? ReviewedById { get; set; }
        public User? ReviewedBy { get; set; }
    }
}
